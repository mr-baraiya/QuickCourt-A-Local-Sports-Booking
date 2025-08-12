using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public TimeSlotController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllTimeSlots
        [HttpGet("All")]
        public async Task<ActionResult<List<TimeSlot>>> GetAllTimeSlots()
        {
            try
            {
                var slots = await _context.TimeSlots.ToListAsync();
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving time slots.", Error = ex.Message });
            }
        }
        #endregion

        #region GetTimeSlotById
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeSlot>> GetTimeSlotById(int id)
        {
            try
            {
                var slot = await _context.TimeSlots.FindAsync(id);
                if (slot == null)
                    return NotFound(new { Message = $"TimeSlot with ID {id} not found." });

                return Ok(slot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving time slot by ID.", Error = ex.Message });
            }
        }
        #endregion

        #region AddTimeSlot
        [HttpPost]
        public async Task<ActionResult<TimeSlot>> AddTimeSlot(TimeSlot slot)
        {
            try
            {
                await _context.TimeSlots.AddAsync(slot);
                await _context.SaveChangesAsync();

                return Ok(slot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error adding time slot.", Error = ex.Message });
            }
        }
        #endregion

        #region UpdateTimeSlot
        [HttpPut("{id}")]
        public async Task<ActionResult<TimeSlot>> UpdateTimeSlot(int id, TimeSlot updatedSlot)
        {
            try
            {
                if (id != updatedSlot.TimeSlotId)
                    return BadRequest(new { Message = "TimeSlot ID mismatch." });

                var existingSlot = await _context.TimeSlots.FindAsync(id);
                if (existingSlot == null)
                    return NotFound(new { Message = $"TimeSlot with ID {id} not found." });

                existingSlot.CourtId = updatedSlot.CourtId;
                existingSlot.SlotDate = updatedSlot.SlotDate;
                existingSlot.StartTime = updatedSlot.StartTime;
                existingSlot.EndTime = updatedSlot.EndTime;
                existingSlot.Status = updatedSlot.Status;

                await _context.SaveChangesAsync();

                return Ok(existingSlot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating time slot.", Error = ex.Message });
            }
        }
        #endregion

        #region DeleteTimeSlot
        [HttpDelete("{id}")]
        public async Task<ActionResult<TimeSlot>> DeleteTimeSlot(int id)
        {
            try
            {
                var slot = await _context.TimeSlots.FindAsync(id);
                if (slot == null)
                    return NotFound(new { Message = $"TimeSlot with ID {id} not found." });

                _context.TimeSlots.Remove(slot);
                await _context.SaveChangesAsync();

                return Ok(slot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting time slot.", Error = ex.Message });
            }
        }
        #endregion

        #region FilterTimeSlots
        [HttpGet("Filter")]
        public async Task<ActionResult<List<TimeSlot>>> FilterTimeSlots(
            [FromQuery] int? courtId,
            [FromQuery] DateOnly? slotDate,
            [FromQuery] TimeOnly? startTime,
            [FromQuery] TimeOnly? endTime,
            [FromQuery] string? status)
        {
            try
            {
                var query = _context.TimeSlots.AsQueryable();

                if (courtId.HasValue)
                    query = query.Where(s => s.CourtId == courtId.Value);

                if (slotDate.HasValue)
                    query = query.Where(s => s.SlotDate == slotDate.Value);

                if (startTime.HasValue)
                    query = query.Where(s => s.StartTime >= startTime.Value);

                if (endTime.HasValue)
                    query = query.Where(s => s.EndTime <= endTime.Value);

                if (!string.IsNullOrWhiteSpace(status))
                {
                    var loweredStatus = status.ToLower();
                    query = query.Where(s => s.Status != null && s.Status.ToLower().Contains(loweredStatus));
                }

                var result = await query.OrderBy(s => s.SlotDate).ThenBy(s => s.StartTime).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering time slots.", Error = ex.Message });
            }
        }
        #endregion

    }
}
