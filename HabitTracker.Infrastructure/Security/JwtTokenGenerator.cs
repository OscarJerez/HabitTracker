using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HabitTracker.Application.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HabitTracker.Infrastructure.Security
{
    // Concrete implementation that creates a signed JWT using settings from configuration.
    // Reads: JwtSettings:Issuer, JwtSettings:Audience, JwtSettings:SigningKey, JwtSettings:ExpiryMinutes.
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Builds a JWT with common identity claims.
        // The token is signed using the symmetric key configured under JwtSettings:SigningKey.
        public string GenerateToken(string userId, string emailAddress, string roleName, out DateTime expiresAtUtc)
        {
            string issuer = _configuration["JwtSettings:Issuer"]
                            ?? throw new InvalidOperationException("JwtSettings:Issuer is not configured.");
            string audience = _configuration["JwtSettings:Audience"]
                              ?? throw new InvalidOperationException("JwtSettings:Audience is not configured.");
            string signingKeyValue = _configuration["JwtSettings:SigningKey"]
                                     ?? throw new InvalidOperationException("JwtSettings:SigningKey is not configured.");

            string expiryMinutesText = _configuration["JwtSettings:ExpiryMinutes"] ?? "60";
            bool parsed = int.TryParse(expiryMinutesText, out int expiryMinutes);
            if (!parsed || expiryMinutes <= 0)
            {
                expiryMinutes = 60; // Sensible default
            }

            DateTime utcNow = DateTime.UtcNow;
            expiresAtUtc = utcNow.AddMinutes(expiryMinutes);

            // Create identity claims for the token.
            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, emailAddress),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Build signing credentials.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyValue));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Assemble the token.
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: utcNow,
                expires: expiresAtUtc,
                signingCredentials: signingCredentials
            );

            // Serialize to compact JWT string.
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
