using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickCourtBackend.Models;
using BCrypt.Net;
using System.Threading.Tasks;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly QuickCourtContext _context;

        #region Constructor
        public UsersController(QuickCourtContext context)
        {
            _context = context;
        }
        #endregion

        #region Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest(new { Message = "Email already exists." });
                }

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    Role = request.Role,
                    IsActive = true,
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "User registered successfully.", user.UserId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error registering user.", Error = ex.Message });
            }
        }
        #endregion

        #region Login User
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                if (user.IsActive == false)
                {
                    return Unauthorized(new { Message = "User account is inactive." });
                }

                // Normally you'd generate a JWT token here
                return Ok(new { Message = "Login successful.", user.UserId, user.FullName, user.Email, user.Role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error logging in.", Error = ex.Message });
            }
        }
        #endregion

        #region Change Password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                    return NotFound(new { Message = "User not found." });

                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                {
                    return BadRequest(new { Message = "Current password is incorrect." });
                }

                user.PasswordHash = HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Password changed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error changing password.", Error = ex.Message });
            }
        }
        #endregion

        #region Helper Methods for Bcrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
        #endregion

        #region Get Total User Count
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalUserCount()
        {
            try
            {
                var count = await _context.Users.CountAsync();
                return Ok(new { TotalUsers = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving user count.", Error = ex.Message });
            }
        }
        #endregion

        #region Get Total Facility Owners Count
        [HttpGet("owners/count")]
        public async Task<IActionResult> GetTotalFacilityOwnersCount()
        {
            try
            {
                // Count distinct OwnerId from Facilities table
                var count = await _context.Facilities
                    .Select(f => f.OwnerId)
                    .Distinct()
                    .CountAsync();

                return Ok(new { TotalFacilityOwners = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving facility owners count.", Error = ex.Message });
            }
        }
        #endregion

    }

    // Request DTOs
    public class UserRegistrationRequest
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "user"; // default role
    }

    public class UserLoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
