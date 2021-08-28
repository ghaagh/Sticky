using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sticky.Application.Dashboard.Settings;
using Sticky.Domain.UserAggrigate.Exceptions;
using Sticky.Infrastructure.Sql;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSetting _setting;
        public TokenGenerator(UserManager<User> userManager, IOptions<JwtSetting> options)
        {
            _userManager = userManager;
            _setting = options.Value;
        }
        public async Task<string> GenerateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException();

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                throw new UserPassIncorrectException();

            var roles = await _userManager.GetRolesAsync(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _setting.Issuer,
            claims: CreateClaims(user, roles),
            expires: DateTime.Now.AddMinutes(_setting.ExpirationInMinutes),
            signingCredentials: credentials

            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> CreateClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            };

            foreach (var item in roles)
                claims.Add(new Claim(ClaimTypes.Role, item));

            return claims;
        }
    }
}
