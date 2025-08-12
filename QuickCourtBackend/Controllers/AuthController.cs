using Microsoft.AspNetCore.Mvc;

namespace QuickCourtBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // POST: api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Dummy role-based login
            if (request.Email == "admin@example.com" && request.Password == "123456")
            {
                return Ok(new
                {
                    token = "fake-jwt-token-admin",
                    role = "Admin",
                    user = new { id = 1, name = "Admin User", email = request.Email }
                });
            }
            else if (request.Email == "owner@example.com" && request.Password == "123456")
            {
                return Ok(new
                {
                    token = "fake-jwt-token-owner",
                    role = "FacilityOwner",
                    user = new { id = 2, name = "Owner User", email = request.Email }
                });
            }
            else if (request.Email == "user@example.com" && request.Password == "123456")
            {
                return Ok(new
                {
                    token = "fake-jwt-token-user",
                    role = "User",
                    user = new { id = 3, name = "Normal User", email = request.Email }
                });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }


        // POST: api/Auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // In a real app: save to DB
            return Ok(new { message = "User registered successfully" });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
