using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public SportController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllSports
        [HttpGet("All")]
        public async Task<ActionResult<List<Sport>>> GetAllSports()
        {
            try
            {
                var sports = await _context.Sports.ToListAsync();
                return Ok(sports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving sports.", Error = ex.Message });
            }
        }
        #endregion

        #region GetSportById
        [HttpGet("{id}")]
        public async Task<ActionResult<Sport>> GetSportById(int id)
        {
            try
            {
                var sport = await _context.Sports.FindAsync(id);
                if (sport == null)
                    return NotFound(new { Message = $"Sport with ID {id} not found." });

                return Ok(sport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving sport by ID.", Error = ex.Message });
            }
        }
        #endregion

        #region AddSport
        [HttpPost]
        public async Task<ActionResult<Sport>> AddSport(Sport sport)
        {
            try
            {
                await _context.Sports.AddAsync(sport);
                await _context.SaveChangesAsync();

                return Ok(sport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error adding sport.", Error = ex.Message });
            }
        }
        #endregion

        #region UpdateSport
        [HttpPut("{id}")]
        public async Task<ActionResult<Sport>> UpdateSport(int id, Sport updatedSport)
        {
            try
            {
                if (id != updatedSport.SportId)
                    return BadRequest(new { Message = "Sport ID mismatch." });

                var existingSport = await _context.Sports.FindAsync(id);
                if (existingSport == null)
                    return NotFound(new { Message = $"Sport with ID {id} not found." });

                existingSport.Name = updatedSport.Name;
                existingSport.IconUrl = updatedSport.IconUrl;

                await _context.SaveChangesAsync();

                return Ok(existingSport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating sport.", Error = ex.Message });
            }
        }
        #endregion

        #region DeleteSport
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sport>> DeleteSport(int id)
        {
            try
            {
                var sport = await _context.Sports.FindAsync(id);
                if (sport == null)
                    return NotFound(new { Message = $"Sport with ID {id} not found." });

                _context.Sports.Remove(sport);
                await _context.SaveChangesAsync();

                return Ok(sport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting sport.", Error = ex.Message });
            }
        }
        #endregion

        #region SportDropdown
        [HttpGet("dropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetSportDropdown()
        {
            try
            {
                var sports = await _context.Sports
                    .Select(s => new
                    {
                        s.SportId,
                        s.Name
                    })
                    .ToListAsync();

                return Ok(sports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching sport dropdown data.", Error = ex.Message });
            }
        }
        #endregion

    }
}
