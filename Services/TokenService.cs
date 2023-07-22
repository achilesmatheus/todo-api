using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Todo.Configurations;
using Todo.Models;

namespace Todo.Services;

public class TokenService
{
    public string GenerateToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtConfiguration.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(GetClaims(user)),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public IEnumerable<Claim> GetClaims(UserModel user)
    {
        var result = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email)
        };
        result.AddRange(
            user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name))
        );
        return result;
    }
}
