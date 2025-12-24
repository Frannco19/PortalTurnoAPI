using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationsServiceOptions _options;

        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticationsServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        private async Task<User?> ValidateUserAsync(LoginRequest rq)
        {
            if (string.IsNullOrWhiteSpace(rq.Email) || string.IsNullOrWhiteSpace(rq.Password))
                return null;

            var user = await _userRepository.GetByEmailAsync(rq.Email);
            if (user == null) return null;

            if (!user.IsActive) return null;

            // MODO CLASE (simple): comparar string (si hoy guardás password en PasswordHash tal cual)
            // Cuando hagamos bien seguridad, acá va hashing.
            if (user.PasswordHash != rq.Password) return null;

            return user;
        }

        public string? Login(LoginRequest rq)
        {
            var user = ValidateUserAsync(rq).GetAwaiter().GetResult();
            if (user == null) return null;

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // "SuperAdmin" / "Admin" / "User"
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claimsForToken,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
