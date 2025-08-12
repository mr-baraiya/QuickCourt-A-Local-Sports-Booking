using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;
using System.Security.Cryptography;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpVerificationController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public OtpVerificationController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region CreateOtp
        [HttpPost]
        public async Task<IActionResult> CreateOtp([FromBody] OtpCreationRequest request)
        {
            try
            {
                // Validate input
                if (request.UserId == null && string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { Message = "Either UserId or Email must be provided." });
                }

                // Generate a 6-digit numeric OTP securely
                var otpCode = GenerateOtpCode(6);

                var otp = new OtpVerification
                {
                    UserId = request.UserId,
                    Email = request.Email,
                    Purpose = request.Purpose,
                    OtpCode = otpCode,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5), // OTP valid for 5 minutes
                    IsUsed = false
                };

                await _context.OtpVerifications.AddAsync(otp);
                await _context.SaveChangesAsync();

                // Return the OTP (In production, send via SMS/Email instead)
                return Ok(new
                {
                    otp.OtpId,
                    otp.UserId,
                    otp.Email,
                    otp.Purpose,
                    OtpCode = otpCode,
                    otp.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                return StatusCode(500, new { Message = "Error creating OTP.", Error = ex.Message, InnerError = innerMessage });
            }
        }

        private string GenerateOtpCode(int length)
        {
            const string digits = "0123456789";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            var otpChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                otpChars[i] = digits[bytes[i] % digits.Length];
            }
            return new string(otpChars);
        }
        #endregion

        #region VerifyOtp
        [HttpPost("Verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                if (request.UserId == null && string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest(new { Message = "Either UserId or Email must be provided." });
                }

                var query = _context.OtpVerifications.AsQueryable();

                if (request.UserId != null)
                {
                    query = query.Where(o => o.UserId == request.UserId);
                }
                else
                {
                    query = query.Where(o => o.Email == request.Email);
                }

                var otp = await query
                    .Where(o => o.Purpose == request.Purpose &&
                                o.OtpCode == request.OtpCode &&
                                o.ExpiresAt > DateTime.UtcNow &&
                                o.IsUsed == false)
                    .OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync();

                if (otp == null)
                    return BadRequest(new { Message = "Invalid or expired OTP." });

                otp.IsUsed = true;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "OTP verified successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error verifying OTP.", Error = ex.Message });
            }
        }
        #endregion

        #region CleanupOtps
        [HttpDelete("Cleanup")]
        public async Task<IActionResult> CleanupOtps()
        {
            try
            {
                var now = DateTime.UtcNow;
                var expiredOtps = _context.OtpVerifications.Where(o => o.IsUsed == true || o.ExpiresAt <= now);
                _context.OtpVerifications.RemoveRange(expiredOtps);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Cleanup completed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error cleaning up OTPs.", Error = ex.Message });
            }
        }
        #endregion
    }
}
