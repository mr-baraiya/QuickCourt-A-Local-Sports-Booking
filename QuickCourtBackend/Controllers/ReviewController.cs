using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public ReviewController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllReviews
        [HttpGet("All")]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            try
            {
                var reviews = await _context.Reviews.ToListAsync();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving reviews.", Error = ex.Message });
            }
        }
        #endregion

        #region GetReviewById
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                    return NotFound(new { Message = $"Review with ID {id} not found." });

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving review by ID.", Error = ex.Message });
            }
        }
        #endregion

        #region AddReview
        [HttpPost]
        public async Task<ActionResult<Review>> AddReview(Review review)
        {
            try
            {
                review.CreatedAt = DateTime.UtcNow;

                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error adding review.", Error = ex.Message });
            }
        }
        #endregion

        #region UpdateReview
        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> UpdateReview(int id, Review updatedReview)
        {
            try
            {
                if (id != updatedReview.ReviewId)
                    return BadRequest(new { Message = "Review ID mismatch." });

                var existingReview = await _context.Reviews.FindAsync(id);
                if (existingReview == null)
                    return NotFound(new { Message = $"Review with ID {id} not found." });

                existingReview.UserId = updatedReview.UserId;
                existingReview.FacilityId = updatedReview.FacilityId;
                existingReview.Rating = updatedReview.Rating;
                existingReview.Comment = updatedReview.Comment;
                // Do not update CreatedAt on update

                await _context.SaveChangesAsync();

                return Ok(existingReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating review.", Error = ex.Message });
            }
        }
        #endregion

        #region DeleteReview
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                    return NotFound(new { Message = $"Review with ID {id} not found." });

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting review.", Error = ex.Message });
            }
        }
        #endregion

        #region FilterReviews
        [HttpGet("Filter")]
        public async Task<ActionResult<List<Review>>> FilterReviews(
            [FromQuery] int? userId,
            [FromQuery] int? facilityId,
            [FromQuery] int? minRating,
            [FromQuery] int? maxRating,
            [FromQuery] string? comment,
            [FromQuery] DateTime? createdAfter,
            [FromQuery] DateTime? createdBefore)
        {
            try
            {
                var query = _context.Reviews.AsQueryable();

                if (userId.HasValue)
                    query = query.Where(r => r.UserId == userId.Value);

                if (facilityId.HasValue)
                    query = query.Where(r => r.FacilityId == facilityId.Value);

                if (minRating.HasValue)
                    query = query.Where(r => r.Rating.HasValue && r.Rating.Value >= minRating.Value);

                if (maxRating.HasValue)
                    query = query.Where(r => r.Rating.HasValue && r.Rating.Value <= maxRating.Value);

                if (!string.IsNullOrWhiteSpace(comment))
                {
                    var loweredComment = comment.ToLower();
                    query = query.Where(r => r.Comment != null && r.Comment.ToLower().Contains(loweredComment));
                }

                if (createdAfter.HasValue)
                    query = query.Where(r => r.CreatedAt.HasValue && r.CreatedAt.Value >= createdAfter.Value);

                if (createdBefore.HasValue)
                    query = query.Where(r => r.CreatedAt.HasValue && r.CreatedAt.Value <= createdBefore.Value);

                var result = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error filtering reviews.", Error = ex.Message });
            }
        }
        #endregion

    }
}
