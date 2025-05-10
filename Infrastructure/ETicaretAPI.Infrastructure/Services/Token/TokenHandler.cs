using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ETicaretAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ETicaretAPI.Infrastructure.Services.Token;

public class TokenHandler: ITokenHandler
{
    readonly IConfiguration _configuration;
    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Application.DTOs.Token CreateAccessToken(int seconds)
    {
        Application.DTOs.Token token = new();
        
        //Security Key'in simetrigini aliyoruz
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        
        //Sifrelenmis kimligi olusturuyoruz.
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        
        //Olusturulacak token ayarlarini veriyoruz.
        token.Expiration = DateTime.UtcNow.AddMinutes(seconds);
        JwtSecurityToken securityToken = new(
            audience:_configuration["Token:Audience"],
            issuer:_configuration["Token:Issuer"],
            expires:token.Expiration,
            notBefore:DateTime.UtcNow,
            signingCredentials:signingCredentials);
        
        //Token olusuturucu sinifindan bir ornek alalim.
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken= tokenHandler.WriteToken(securityToken);
        return token;
    }
}