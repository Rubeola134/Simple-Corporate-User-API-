using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Model;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class AuthHelper(IConfiguration configuration)
    {
        
        public string CreateToken(User user)
        {
            Claim[] claims = new Claim[]
            {
            new Claim("userId", user.UserId.ToString())
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:TokenKey"] ?? throw new Exception("Missing PasswordKey")));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}