using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Your DbContext namespace
using QuickCourtBackend.Models;
using System.Linq;
using System.Threading.Tasks;
namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "admin")] // IMPORTANT: Secure this controller for admins only!
    public class AdminController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        public AdminController(QuickCourtContext context)
        {
            _context = context;
        }

        // === DASHBOARD APIS ===

        // 1. Endpoint for Global Stats Cards
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalFacilityOwners = await _context.Users.CountAsync(u => u.Role == "facilityOwner"),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalActiveCourts = await _context.Courts.CountAsync(c => c.IsActive == true)
            };
            return Ok(stats);
        }

        // 2. Endpoint for Booking Activity Chart (bookings per day)
        [HttpGet("dashboard/charts/booking-activity")]
        public async Task<IActionResult> GetBookingActivity()
        {
            var bookingActivity = await _context.Bookings
                .GroupBy(b => b.StartTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();
            return Ok(bookingActivity);
        }

        // 3. Endpoint for Most Active Sports Chart
        [HttpGet("dashboard/charts/most-active-sports")]
        public async Task<IActionResult> GetMostActiveSports()
        {
            var mostActiveSports = await _context.Bookings
                .Include(b => b.Court)
                .ThenInclude(c => c.Sport)
                .GroupBy(b => b.Court.Sport.Name)
                .Select(g => new { SportName = g.Key, BookingCount = g.Count() })
                .OrderByDescending(x => x.BookingCount)
                .ToListAsync();

            return Ok(mostActiveSports);
        }


        // === FACILITY APPROVAL APIS ===

        // 1. Endpoint to get all facilities with "pending" status
        [HttpGet("facilities/pending")]
        public async Task<IActionResult> GetPendingFacilities()
        {
            var pendingFacilities = await _context.Facilities
                .Where(f => f.Status == "pending")
                .Include(f => f.Owner) // Include User details of the owner
                .Select(f => new {
                    f.FacilityId,
                    f.Name,
                    f.Address,
                    f.City,
                    f.State,
                    f.CreatedAt,
                    OwnerName = f.Owner.FullName
                })
                .ToListAsync();

            return Ok(pendingFacilities);
        }

        // 2. Endpoint to approve or reject a facility
        [HttpPut("facilities/{facilityId}/status")]
        public async Task<IActionResult> UpdateFacilityStatus(int facilityId, [FromBody] FacilityStatusUpdateDto request)
        {
            var facility = await _context.Facilities.FindAsync(facilityId);
            if (facility == null)
            {
                return NotFound("Facility not found.");
            }

            // var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in admin's ID

            facility.Status = request.NewStatus; // "approved" or "rejected"
            facility.ApprovalComments = request.Comments;
            // facility.ApprovedBy = int.Parse(adminId);
            facility.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Facility status updated to {request.NewStatus}." });
        }
    }

    // DTO for the status update request body
    public class FacilityStatusUpdateDto
    {
        public string NewStatus { get; set; } // "approved" or "rejected"
        public string Comments { get; set; }
    }
}
