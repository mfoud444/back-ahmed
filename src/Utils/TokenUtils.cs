using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend_Teamwork.src.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Backend_Teamwork.src.Utils
{
    public class TokenUtils
    {
        private readonly IConfiguration _configuration;

        public TokenUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!)
            );
            var SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256Signature
            );

            // subject = list of claims, never put password in claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
            // use _config to access value in appsetting
            var issure = _configuration.GetSection("Jwt:Issuer").Value;
            var audience = _configuration.GetSection("Jwt:Audience").Value;
            var descriptor = new SecurityTokenDescriptor
            {
                // header
                Issuer = issure,
                Audience = audience,
                Expires = DateTime.Now.AddMinutes(60), // handle date here

                // payload
                Subject = new ClaimsIdentity(claims),

                // verify
                SigningCredentials = SigningCredentials,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
