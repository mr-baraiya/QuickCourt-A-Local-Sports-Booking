using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models; // Your models namespace

namespace QuickCourtBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly QuickCourtContext _context; // Your DbContext

        public BookingController(QuickCourtContext context)
        {
            _context = context;
        }

        // GET: api/booking/available-slots?facilityId=1&date=2025-08-12
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] int facilityId, [FromQuery] DateTime date)
        {
            try
            {
                var dateOnly = date.Date;

                // Find all time slots for the given facility and date that are marked 'available'
                var availableSlots = await _context.TimeSlots
                    .Include(ts => ts.Court) // Join Court table
                    .ThenInclude(c => c.Sport) // Then join Sport table from Court
                    .Where(ts => ts.Court.FacilityId == facilityId && ts.Status == "available")
                    .ToListAsync();

                // Find all confirmed bookings for that day to exclude those slots
                var confirmedBookings = await _context.Bookings
                    .Where(b => b.StartTime.Date == dateOnly && b.Court.FacilityId == facilityId && b.Status == "confirmed")
                    .ToListAsync();

                // Filter out the slots that are already booked
                var trulyAvailableSlots = availableSlots.Where(ts => !confirmedBookings.Any(b =>
                    b.CourtId == ts.CourtId
                )).Select(ts => new
                {
                    // Select the data needed by the frontend
                    courtId = ts.CourtId,
                    courtName = ts.Court.Name,
                    pricePerHour = ts.Court.PricePerHour,
                    sportName = ts.Court.Sport.Name,
                    timeSlotId = ts.TimeSlotId,
                    startTime = ts.StartTime.ToString(@"hh\:mm"),
                    endTime = ts.EndTime.ToString(@"hh\:mm")
                }).ToList();


                if (!trulyAvailableSlots.Any())
                {
                    return NotFound("No available slots found for the selected facility and date.");
                }

                return Ok(trulyAvailableSlots);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(500, "An internal error occurred while fetching slots.");
            }
        }

        // POST: api/booking/create
        // POST: api/booking/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- START OF FIX ---

            // Define the expected time format
            string timeFormat = "hh\\:mm";

            if (!TimeSpan.TryParseExact(bookingRequest.StartTime, timeFormat, null, out TimeSpan parsedStartTime) ||
                !TimeSpan.TryParseExact(bookingRequest.EndTime, timeFormat, null, out TimeSpan parsedEndTime))
            {
                // If parsing fails, return a helpful error message
                return BadRequest($"Invalid time format. Please use the '{timeFormat}' format for StartTime and EndTime.");
            }

            // Combine date and the successfully parsed time
            var startTime = bookingRequest.Date.Date.Add(parsedStartTime);
            var endTime = bookingRequest.Date.Date.Add(parsedEndTime);

            // --- END OF FIX ---


            // This part remains the same
            var userId = 2; // Replace with actual authenticated user ID.

            var newBooking = new Booking
            {
                UserId = userId,
                CourtId = bookingRequest.CourtId,
                StartTime = startTime,
                EndTime = endTime,
                TotalPrice = bookingRequest.TotalPrice,
                Status = "confirmed",
                PaymentStatus = "pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Bookings.Add(newBooking);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Booking created successfully!", bookingId = newBooking.BookingId });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while saving the booking.");
            }
        }
    }

    // DTO (Data Transfer Object) for the booking request from the frontend.
    // This simplifies what the frontend needs to send.
    public class BookingDto
    {
        public int CourtId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; } // e.g., "09:00"
        public string EndTime { get; set; }   // e.g., "10:00"
    }
}

