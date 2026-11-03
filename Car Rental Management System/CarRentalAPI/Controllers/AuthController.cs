using CarRentalAPIBusinessLayer;
using CarRentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRentalAPI.Controllers
{

    //Authentication

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GenerateAccessToken(clsUser User)
        {
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

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        //A secure system is not only protected — it is observable.
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [HttpPost("login")]
        [EnableRateLimiting("AuthLimiter")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var User = clsUser.Find(request.UserName);

            if (User == null)
            {
                _logger.LogWarning("Failed login attempt (UserName not found). UserName={UserName}, IP={IP}",
                                    request.UserName,IP);
                
                return Unauthorized("Invalid Credentials");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, User.PasswordHash);


            if (!isValidPassword || !User.IsActive)
            {
                _logger.LogWarning("Failed login attempt (UserName not found). UserName={UserName}, IP={IP}",
                                     request.UserName,IP);
               
                return Unauthorized("Invalid Credentials");

            }
                

            clsUser.UpdateLastLoginDate(User.UserID);


            // Create Access Token (random)
            var accessToken = GenerateAccessToken(User);

            // Create refresh token (random)
            var refreshToken = GenerateRefreshToken();

            // Store refresh token securely (hash + expiry + not revoked)
            User.RefreshTokenHash      = BCrypt.Net.BCrypt.HashPassword(refreshToken);
            User.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            User.RefreshTokenRevokedAt = null;

            if (!User.SaveRefreshTokenData())
                return StatusCode(500, "Could not complete");

            _logger.LogInformation("Successful login. UserID={UserID}, UserName={UserName}, IP={IP}", User.UserID, User.UserName, IP);

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });

        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthLimiter")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            var IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var User = clsUser.Find(request.UserName);

            if (User == null || !User.IsActive)
            {
                _logger.LogWarning("Failed Refresh Token attempt (UserName not found Or Inactive). UserName={UserName}, IP={IP}",
                                    request.UserName, IP);

                 return Unauthorized("Invalid refresh request");
            }
             
            if (User.RefreshTokenRevokedAt != null)
                return Unauthorized("Refresh token is revoked");

            if (User.RefreshTokenExpiresAt == null || User.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            if (string.IsNullOrEmpty(User.RefreshTokenHash))
                return Unauthorized("Invalid refresh request");

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, User.RefreshTokenHash);

            if (!refreshValid)
                return Unauthorized("Invalid refresh token");

            // Issue NEW access token (same claims & signing settings as login)
            // Create Access Token (random)
            var newAccessToken = GenerateAccessToken(User);

            // Rotation: replace refresh token
            var newRefreshToken = GenerateRefreshToken();

            User.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            User.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            User.RefreshTokenRevokedAt = null;

            if (!User.SaveRefreshTokenData())
                return StatusCode(500, "Could not complete");

            

            //_logger.LogInformation("Successful Refresh Token. UserID={UserID}, UserName={UserName}, IP={IP}", User.UserID, User.UserName, IP);

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogoutRequest request)
        {
            var User = clsUser.Find(request.UserName);

            if (User == null)
                return Ok(); // Do not reveal if user exists

            bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, User.RefreshTokenHash);
            if (!refreshValid)
                return Ok();

            User.RefreshTokenRevokedAt = DateTime.UtcNow;

            if (!User.SaveRefreshTokenData())
                return StatusCode(500, "Could not complete logout");

            return Ok("Logged out successfully");
        }


    }
}
