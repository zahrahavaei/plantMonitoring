using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantMonitorring.DBContext;
using PlantMonitorring.Entity;
using PlantMonitorring.Enum;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using PlantMonitorring.Models;

namespace PlantMonitorring.Controllers
{
    [AllowAnonymous]
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly PlantDataBaseContext _context;
        private readonly ILogger<LoginController> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(PlantDataBaseContext context, 
               ILogger<LoginController> logger,
               IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }
        private string GenerateJwtToken(int userId, string username, string fullName, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                ("my_super_strong_secret_key_1234567890@JWT!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(ClaimTypes.Name, fullName),
        new Claim(ClaimTypes.Role, role),
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
          };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7245",
                audience: "https://localhost:7245",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);//convert token json object to json string
        }
        [HttpPost()]
        public async Task<IActionResult> LoginAsync( UserDtoPost dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
            {
                _logger.LogWarning("Login failed: Invalid user credentials provided.");
                return BadRequest("Username and password are required.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync
                                  (u => u.UserName == dto.Username);
            if (existingUser == null)
            {
                _logger.LogWarning("Login failed for user {Username}: User not found", dto.Username);
                return Unauthorized("Invalid username or password.");
            }
            var userHasher = new PasswordHasher<User>();

            var verifyPasswod = userHasher.VerifyHashedPassword
                              (existingUser, existingUser.Password, dto.Password);
            if (verifyPasswod == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed for user {Username}: Invalid password", dto.Username);
                return Unauthorized("Invalid username or password.");
            }
            var token = GenerateJwtToken(existingUser.Id,
                existingUser.UserName, existingUser.Name, existingUser.UserRole.ToString());
            var userDtoResponse = new UserDtoResponseLogin
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                UserName = existingUser.UserName,
                Email = existingUser.Email,
                UserRole = existingUser.UserRole
            };
            return Ok(new
            {
                token,
                userDtoResponse

            });
        }
    }
}
