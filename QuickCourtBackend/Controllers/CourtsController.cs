using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtsController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public CourtsController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetCourts()
        {
            try
            {
                var courts = await _context.Courts.ToListAsync();
                return Ok(courts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving courts.", Error = ex.Message });
            }
        }
        #endregion

        #region GetById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourt(int id)
        {
            try
            {
                var court = await _context.Courts.FindAsync(id);
                if (court == null) return NotFound(new { Message = "Court not found." });

                return Ok(court);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving court.", Error = ex.Message });
            }
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<IActionResult> CreateCourt([FromBody] Court court)
        {
            try
            {
                court.CreatedAt = DateTime.UtcNow;
                court.UpdatedAt = DateTime.UtcNow;

                _context.Courts.Add(court);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCourt), new { id = court.CourtId }, court);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating court.", Error = ex.Message });
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourt(int id, [FromBody] Court court)
        {
            if (id != court.CourtId) return BadRequest(new { Message = "ID mismatch." });

            try
            {
                var existingCourt = await _context.Courts.FindAsync(id);
                if (existingCourt == null) return NotFound(new { Message = "Court not found." });

                // Update fields
                existingCourt.Name = court.Name;
                existingCourt.FacilityId = court.FacilityId;
                existingCourt.SportId = court.SportId;
                existingCourt.PricePerHour = court.PricePerHour;
                existingCourt.Capacity = court.Capacity;
                existingCourt.IsActive = court.IsActive;
                existingCourt.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(existingCourt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating court.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourt(int id)
        {
            try
            {
                var court = await _context.Courts.FindAsync(id);
                if (court == null) return NotFound(new { Message = "Court not found." });

                _context.Courts.Remove(court);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Court deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting court.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter
        [HttpPost("filter")]
        public async Task<IActionResult> FilterCourts([FromBody] Court filter)
        {
            try
            {
                var query = _context.Courts.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(c => c.Name.Contains(filter.Name));

                if (filter.FacilityId != 0)
                    query = query.Where(c => c.FacilityId == filter.FacilityId);

                if (filter.SportId != 0)
                    query = query.Where(c => c.SportId == filter.SportId);

                if (filter.PricePerHour > 0)
                    query = query.Where(c => c.PricePerHour == filter.PricePerHour);

                if (filter.Capacity.HasValue)
                    query = query.Where(c => c.Capacity == filter.Capacity);

                if (filter.IsActive.HasValue)
                    query = query.Where(c => c.IsActive == filter.IsActive);

                var result = await query.ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering courts.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Courts by Facility
        // GET /api/facilities/{facilityId}/courts
        [HttpGet("/api/facilities/{facilityId}/courts")]
        public async Task<IActionResult> GetCourtsByFacility(int facilityId)
        {
            try
            {
                var courts = await _context.Courts
                    .Where(c => c.FacilityId == facilityId)
                    .ToListAsync();

                return Ok(courts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching courts for facility.", Error = ex.Message });
            }
        }
        #endregion

        #region Activate Court
        // PATCH /api/courts/{id}/activate
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateCourt(int id)
        {
            try
            {
                var court = await _context.Courts.FindAsync(id);
                if (court == null)
                    return NotFound(new { Message = $"Court with ID {id} not found." });

                court.IsActive = true;
                court.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Court activated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error activating court.", Error = ex.Message });
            }
        }
        #endregion

        #region Deactivate Court
        // PATCH /api/courts/{id}/deactivate
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateCourt(int id)
        {
            try
            {
                var court = await _context.Courts.FindAsync(id);
                if (court == null)
                    return NotFound(new { Message = $"Court with ID {id} not found." });

                court.IsActive = false;
                court.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Court deactivated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deactivating court.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Total Active Courts Count
        [HttpGet("courts/active/count")]
        public async Task<IActionResult> GetTotalActiveCourtsCount()
        {
            try
            {
                var count = await _context.Courts
                    .Where(c => c.IsActive == true)
                    .CountAsync();

                return Ok(new { TotalActiveCourts = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving active courts count.", Error = ex.Message });
            }
        }
        #endregion

    }
}
