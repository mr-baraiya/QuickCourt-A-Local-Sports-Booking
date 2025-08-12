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
    public class FacilityOwnerController : Controller
    {
        private readonly QuickCourtContext _context;

        public FacilityOwnerController(QuickCourtContext context)
        {
            _context = context;
        }
        [HttpGet("dashboard/stats/{ownerId}")]
        public async Task<IActionResult> GetDashboardStats(int ownerId)
        {
            var facilities = await _context.Facilities
                .Where(f => f.OwnerId == ownerId)
                .Include(f => f.Courts)
                .ToListAsync();

            if (!facilities.Any())
                return NotFound("No facilities found for owner");

            // Filter facilities with Status == "Active"
            var activeFacilities = facilities.Where(f => f.Status == "Active").ToList();

            // Get all courts belonging to active facilities
            var activeCourtIds = activeFacilities.SelectMany(f => f.Courts.Select(c => c.CourtId)).ToList();

            // Total bookings for all courts (regardless of facility status)
            var allCourtIds = facilities.SelectMany(f => f.Courts.Select(c => c.CourtId)).ToList();

            var totalBookings = await _context.Bookings
                .Where(b => allCourtIds.Contains(b.CourtId))
                .CountAsync();

            // Active courts count is the count of courts under active facilities
            var activeCourts = activeCourtIds.Count;

            //// Earnings from bookings under all courts of this owner
            //var earnings = await _context.Payments
            //    .Where(p => p.Booking != null && allCourtIds.Contains(p.Booking.CourtId))
            //    .SumAsync(p => (decimal?)p.Amount) ?? 0m;

            return Ok(new
            {
                TotalBookings = totalBookings,
                ActiveCourts = activeCourts
                //Earnings = earnings
            });
        }

      
        [HttpGet("dashboard/bookings/{ownerId}")]
        public async Task<IActionResult> GetBookingTrends(int ownerId, [FromQuery] string range = "monthly")
        {
            var activeFacilities = await _context.Facilities
                .Where(f => f.OwnerId == ownerId && f.Status == "Active")
                .Include(f => f.Courts)
                .ToListAsync();

            if (!activeFacilities.Any())
                return NotFound("No active facilities found for owner");

            var activeCourtIds = activeFacilities.SelectMany(f => f.Courts.Select(c => c.CourtId)).ToList();

            var bookingsQuery = _context.Bookings.Where(b => activeCourtIds.Contains(b.CourtId));

            if (range == "daily")
            {
                var result = await bookingsQuery
                    .GroupBy(b => b.StartTime.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new { Period = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
                    .ToListAsync();

                return Ok(result);
            }
            else // monthly or default
            {
                var result = await bookingsQuery
                    .GroupBy(b => new { b.StartTime.Year, b.StartTime.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Count = g.Count() })
                    .ToListAsync();

                return Ok(result);
            }
        }



    }
}

