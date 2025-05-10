namespace ETicaretAPI.Application.Abstractions.Services.Authentications;

public interface IExternalAuthentication
{
    Task<DTOs.Token> FacebookLoginAsync(string AuthToken,int accessTokenLifetime);
    Task<DTOs.Token> GoogleLoginAsync(string idToken,int accessTokenLifetime);
}