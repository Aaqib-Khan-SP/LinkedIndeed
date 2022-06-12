using LinkedIndeed.BLL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LinkedIndeed.API.Services
{
    public class Auth : IAuth
    {
        private SymmetricSecurityKey _key;
        private JwtSecurityTokenHandler tokenHandler;
        public Auth(SymmetricSecurityKey key)
        {
            _key = key;
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public string NewToken(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId",userId)
                }),

                //for demo purpose only, Sliding token principle can be used here
                Expires = DateTime.UtcNow.AddHours(100),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal VerifyToken(string token)
        {
            var claims = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validateToken);
            return claims;
        }

    }
}
