using System;
using System.Threading.Tasks;
using CompanyDealer.DAL.Models;
using CompanyDealer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyDealer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int Role { get; set; }
            public string Username { get; set; } = string.Empty;
            public bool IsActive { get; set; }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required");
            }

            var result = await _authService.LoginAsync(request.Username, request.Password);
            if (!result.Success)
            {
                return Unauthorized(result.Error);
            }

            return Ok(new LoginResponse
            {
                Id = result.AccountId!.Value,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role,
                Username = result.Username,
                IsActive = result.IsActive
            });
        }
    }
}
