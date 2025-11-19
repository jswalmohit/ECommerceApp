using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceApp.EComm.Services.Implementation
{
    public class AuthService(IAuthRepo authRepo, IConfiguration configuration) : IAuthService
    {
        private readonly IAuthRepo _authRepo = authRepo;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResponse?> GenerateJwtTokenAsync(LoginRequest loginRequest)
        {
            // Validate user credentials
            var user = await _authRepo.ValidateUserAsync(loginRequest.LoginId, loginRequest.Password);
            
            if (user == null)
                return null;

            // Get JWT settings from configuration
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "ECommerceApp";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "ECommerceApp";
            var jwtExpiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

            // Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LoginId),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtExpiryMinutes),
                signingCredentials: credentials
            );

            // Generate token string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                ExpiresAt = DateTime.UtcNow.AddMinutes(jwtExpiryMinutes),
                UserId = user.Id,
                LoginId = user.LoginId
            };
        }
    }
}

