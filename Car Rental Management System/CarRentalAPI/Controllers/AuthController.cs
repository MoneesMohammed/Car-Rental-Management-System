using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Controllers
{

    //Authentication

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var User = clsUser.Find(request.UserName);

            if (User == null)
                return Unauthorized("Invalid credentials");

            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(request.Password, User.PasswordHash);

            if (!isValidPassword)
                return Unauthorized("Invalid credentials");

            clsUser.UpdateLastLoginDate(User.UserID);

            var claims = new[]
            {
               
                new Claim(ClaimTypes.NameIdentifier, User.UserID.ToString()),

                new Claim(ClaimTypes.Name, User.UserName),

                new Claim(ClaimTypes.Role, User.RoleName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "CarRentalApi",
                audience: "CarRentalApiUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
