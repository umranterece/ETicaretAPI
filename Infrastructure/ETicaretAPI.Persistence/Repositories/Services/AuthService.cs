using System.Security.Authentication;
using System.Text.Json;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Persistence.Repositories.Services;

public class AuthService:IAuthService
{
    readonly HttpClient _httpClient;
    readonly IConfiguration _configuration;
    readonly UserManager<AppUser> _userManager;
    readonly ITokenHandler _tokenHandler;
    readonly SignInManager<AppUser> _signInManager;

    public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _httpClient = httpClientFactory.CreateClient();
    }

    async Task<Token> CreateUserExternalAsync(AppUser user,string email, string name, UserLoginInfo info, int accessTokenLifetime)
    {
        bool result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName =email,
                    NameSurname = name
                };
                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }

        if (result)
        {
            await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifetime);
            return token;
        }
        throw new Exception("Invalid external authentication.");

    }
        
    public async Task<Token> FacebookLoginAsync(string AuthToken,int accessTokenLifetime)
    {
        string accessTokenResponse = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

        FacebookAccessTokenResponse? facebookAccessTokenResponse =
            JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

        string userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={AuthToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

        FacebookUserAccessTokenValidation? validation =
            JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
        
        if (validation?.Data.IsValid != null)
        {
            string userInfoResponse =
                await _httpClient.GetStringAsync(
                    $"https://graph.facebook.com/me?fields=email,name&access_token={AuthToken}");

            FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            Domain.Entities.Identity.AppUser user =
                await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            
            return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifetime);
        }
        throw new Exception("Invalid external authentication.");
    }

    public async Task<Token> GoogleLoginAsync(string idToken,int accessTokenLifetime)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { _configuration["ExternalLoginSettings:Google:Client_ID"] ?? string.Empty }
        };

        var payload=await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
        Domain.Entities.Identity.AppUser user= await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
     
        return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifetime);
    }


    public async Task<Token> LoginAsync(string usernameOrEmail, string password, int lifetime)
    {
        Domain.Entities.Identity.AppUser user= await _userManager.FindByNameAsync(usernameOrEmail);
        if (user == null)
            user= await _userManager.FindByEmailAsync(usernameOrEmail);

        if (user == null)
            throw new NotFoundUserException();
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.Succeeded) 
        {
            Token token = _tokenHandler.CreateAccessToken(lifetime);
            return token;
        }
       
        throw new AuthenticationException();
    }
}