using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public FacilitiesController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All Facilities
        [HttpGet]
        public async Task<IActionResult> GetFacilities()
        {
            try
            {
                var facilities = await _context.Facilities.ToListAsync();
                return Ok(facilities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving facilities.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Facility By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacility(int id)
        {
            try
            {
                var facility = await _context.Facilities.FindAsync(id);

                if (facility == null)
                    return NotFound(new { Message = $"Facility with ID {id} not found." });

                return Ok(facility);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving facility.", Error = ex.Message });
            }
        }
        #endregion

        #region Create Facility
        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromBody] Facility facility)
        {
            try
            {
                facility.CreatedAt = DateTime.UtcNow;
                _context.Facilities.Add(facility);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFacility), new { id = facility.FacilityId }, facility);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating facility.", Error = ex.Message });
                //var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                //return StatusCode(500, new { Message = "Error creating facility.", Error = ex.Message, InnerError = innerMessage });
            }
        }
        #endregion

        #region Update Facility
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, [FromBody] Facility updatedFacility)
        {
            try
            {
                if (id != updatedFacility.FacilityId)
                    return BadRequest(new { Message = "Facility ID mismatch." });

                var facility = await _context.Facilities.FindAsync(id);
                if (facility == null)
                    return NotFound(new { Message = $"Facility with ID {id} not found." });

                // Update allowed fields
                facility.Name = updatedFacility.Name;
                facility.Description = updatedFacility.Description;
                facility.Address = updatedFacility.Address;
                facility.City = updatedFacility.City;
                facility.State = updatedFacility.State;
                facility.Status = updatedFacility.Status;
                facility.RejectionReason = updatedFacility.RejectionReason;
                facility.OperatingHoursStart = updatedFacility.OperatingHoursStart;
                facility.OperatingHoursEnd = updatedFacility.OperatingHoursEnd;
                facility.ApprovedBy = updatedFacility.ApprovedBy;
                facility.ApprovalComments = updatedFacility.ApprovalComments;
                facility.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(facility);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating facility.", Error = ex.Message });
            }
        }
        #endregion

        #region Delete Facility
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            try
            {
                var facility = await _context.Facilities.FindAsync(id);
                if (facility == null)
                    return NotFound(new { Message = $"Facility with ID {id} not found." });

                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Facility with ID {id} deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting facility.", Error = ex.Message });
            }
        }
        #endregion

        #region Filter Facilities
        [HttpGet("filter")]
        public async Task<IActionResult> FilterFacilities(
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] string? state,
            [FromQuery] string? status,
            [FromQuery] int? ownerId)
        {
            try
            {
                var query = _context.Facilities.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(f => EF.Functions.Like(f.Name, $"%{name}%"));

                if (!string.IsNullOrWhiteSpace(city))
                    query = query.Where(f => f.City != null && f.City.ToLower() == city.ToLower());

                if (!string.IsNullOrWhiteSpace(state))
                    query = query.Where(f => f.State != null && f.State.ToLower() == state.ToLower());

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(f => f.Status != null && f.Status.ToLower() == status.ToLower());

                if (ownerId.HasValue && ownerId.Value > 0)
                    query = query.Where(f => f.OwnerId == ownerId.Value);

                var filteredFacilities = await query.ToListAsync();

                return Ok(filteredFacilities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering facilities.", Error = ex.Message });
            }
        }
        #endregion

    }
}
