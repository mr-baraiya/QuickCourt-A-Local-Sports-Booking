using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityPhotosController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public FacilityPhotosController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region GetPhotosByFacility
        [HttpGet("byfacility/{facilityId}")]
        public async Task<IActionResult> GetPhotosByFacility(int facilityId)
        {
            try
            {
                var photos = await _context.FacilityPhotos
                    .Where(p => p.FacilityId == facilityId)
                    .ToListAsync();

                return Ok(photos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching photos.", Error = ex.Message });
            }
        }
        #endregion

        #region AddPhoto
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromBody] FacilityPhoto photo)
        {
            try
            {
                await _context.FacilityPhotos.AddAsync(photo);
                await _context.SaveChangesAsync();
                return Ok(photo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error adding photo.", Error = ex.Message });
            }
        }
        #endregion

        #region DeletePhoto
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            try
            {
                var photo = await _context.FacilityPhotos.FindAsync(photoId);
                if (photo == null)
                    return NotFound(new { Message = $"Photo with ID {photoId} not found." });

                _context.FacilityPhotos.Remove(photo);
                await _context.SaveChangesAsync();

                return Ok(photo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting photo.", Error = ex.Message });
            }
        }
        #endregion

        #region UpdatePhoto
        [HttpPut("{photoId}")]
        public async Task<IActionResult> UpdatePhoto(int photoId, [FromBody] FacilityPhoto updatedPhoto)
        {
            try
            {
                if (photoId != updatedPhoto.PhotoId)
                    return BadRequest(new { Message = "Photo ID mismatch." });

                var photo = await _context.FacilityPhotos.FindAsync(photoId);
                if (photo == null)
                    return NotFound(new { Message = $"Photo with ID {photoId} not found." });

                photo.PhotoUrl = updatedPhoto.PhotoUrl;
                photo.Caption = updatedPhoto.Caption;

                await _context.SaveChangesAsync();

                return Ok(photo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating photo.", Error = ex.Message });
            }
        }
        #endregion
        #region Get All Facilities
        [HttpGet]
        public async Task<IActionResult> GetFacilitiesPhoto()
        {
            try
            {
                var facilities = await _context.FacilityPhotos.ToListAsync();
                return Ok(facilities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving facilities.", Error = ex.Message });
            }
        }
        #endregion
    }
}
