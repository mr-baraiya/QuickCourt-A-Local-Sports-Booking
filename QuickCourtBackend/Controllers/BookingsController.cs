using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public BookingsController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region Create Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            try
            {
                booking.CreatedAt = DateTime.UtcNow;

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBookingById), new { bookingId = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating booking.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Booking By Id
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Court)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);

                if (booking == null)
                    return NotFound(new { Message = $"Booking with ID {bookingId} not found." });

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching booking.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Bookings For User
        [HttpGet("/api/users/{userId}/bookings")]
        public async Task<IActionResult> GetBookingsForUser(int userId)
        {
            try
            {
                var bookings = await _context.Bookings
                    .Where(b => b.UserId == userId)
                    .Include(b => b.Court)
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching bookings for user.", Error = ex.Message });
            }
        }
        #endregion

        #region Update Booking Status

        [HttpPatch("{bookingId}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, [FromBody] string newStatus)
        {
            try
            {
                // Validate input status
                var allowedStatuses = new[] { "confirmed", "cancelled", "completed" };
                if (!allowedStatuses.Contains(newStatus.ToLower()))
                    return BadRequest(new { Message = $"Invalid status. Allowed values: {string.Join(", ", allowedStatuses)}" });

                // Find booking
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return NotFound(new { Message = $"Booking with id {bookingId} not found." });

                // Update status
                booking.Status = newStatus.ToLower();
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Booking status updated successfully.", bookingId, newStatus = booking.Status });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                return StatusCode(500, new { Message = "Error updating booking status.", Error = ex.Message, InnerError = innerMessage });
            }
        }

        #endregion

        #region List All Bookings (Admin)
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Court)
                    .Include(b => b.User)
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching all bookings.", Error = ex.Message });
            }
        }
        #endregion

        #region FilterBookings
        [HttpGet("filter")]
        public async Task<ActionResult<List<Booking>>> FilterBookings(
            [FromQuery] int? bookingId,
            [FromQuery] int? userId,
            [FromQuery] int? courtId,
            [FromQuery] DateTime? startTimeFrom,
            [FromQuery] DateTime? startTimeTo,
            [FromQuery] DateTime? endTimeFrom,
            [FromQuery] DateTime? endTimeTo,
            [FromQuery] decimal? minTotalPrice,
            [FromQuery] decimal? maxTotalPrice,
            [FromQuery] string? status,
            [FromQuery] string? paymentStatus,
            [FromQuery] string? cancellationReason,
            [FromQuery] DateTime? createdAtFrom,
            [FromQuery] DateTime? createdAtTo)
        {
            try
            {
                var query = _context.Bookings.AsQueryable();

                if (bookingId.HasValue)
                    query = query.Where(b => b.BookingId == bookingId.Value);

                if (userId.HasValue)
                    query = query.Where(b => b.UserId == userId.Value);

                if (courtId.HasValue)
                    query = query.Where(b => b.CourtId == courtId.Value);

                if (startTimeFrom.HasValue)
                    query = query.Where(b => b.StartTime >= startTimeFrom.Value);

                if (startTimeTo.HasValue)
                    query = query.Where(b => b.StartTime <= startTimeTo.Value);

                if (endTimeFrom.HasValue)
                    query = query.Where(b => b.EndTime >= endTimeFrom.Value);

                if (endTimeTo.HasValue)
                    query = query.Where(b => b.EndTime <= endTimeTo.Value);

                if (minTotalPrice.HasValue)
                    query = query.Where(b => b.TotalPrice >= minTotalPrice.Value);

                if (maxTotalPrice.HasValue)
                    query = query.Where(b => b.TotalPrice <= maxTotalPrice.Value);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(b => b.Status == status);

                if (!string.IsNullOrWhiteSpace(paymentStatus))
                    query = query.Where(b => b.PaymentStatus == paymentStatus);

                if (!string.IsNullOrWhiteSpace(cancellationReason))
                    query = query.Where(b => b.CancellationReason != null && b.CancellationReason.Contains(cancellationReason));

                if (createdAtFrom.HasValue)
                    query = query.Where(b => b.CreatedAt >= createdAtFrom.Value);

                if (createdAtTo.HasValue)
                    query = query.Where(b => b.CreatedAt <= createdAtTo.Value);

                var result = await query
                    .Include(b => b.Court)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while filtering bookings.",
                    Error = ex.Message
                });
            }
        }
        #endregion

        #region Get Total Booking Count
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalBookingCount()
        {
            try
            {
                var count = await _context.Bookings.CountAsync();
                return Ok(new { TotalBookings = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving booking count.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Earnings
        [HttpGet("earnings")]
        public async Task<IActionResult> GetEarnings(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var query = _context.Bookings.AsQueryable();

                // Filter only paid bookings
                query = query.Where(b => b.PaymentStatus == "paid");

                if (startDate.HasValue)
                {
                    query = query.Where(b => b.CreatedAt >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(b => b.CreatedAt <= endDate.Value);
                }

                var totalEarnings = await query.SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

                return Ok(new { TotalEarnings = totalEarnings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error calculating earnings.", Error = ex.Message });
            }
        }
        #endregion

        #region Booking Calendar
        [HttpGet("bookings-calendar")]
        public async Task<IActionResult> GetBookingsCalendar(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var query = _context.Bookings
                    .Include(b => b.Court)
                    .AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(b => b.StartTime >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(b => b.EndTime <= endDate.Value);

                var bookingsCalendar = await query.Select(b => new
                {
                    b.BookingId,
                    b.StartTime,
                    b.EndTime,
                    b.Status,
                    b.PaymentStatus,
                    Court = new
                    {
                        b.Court.CourtId,
                        b.Court.Name,
                        b.Court.PricePerHour,
                        b.Court.Capacity
                    }
                }).ToListAsync();

                return Ok(bookingsCalendar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error fetching booking calendar.",
                    Error = ex.Message
                });
            }
        }
        #endregion

    }
}
