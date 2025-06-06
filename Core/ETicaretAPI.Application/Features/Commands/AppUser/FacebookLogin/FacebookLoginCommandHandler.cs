using System.Text.Json;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    private readonly IAuthService _authService;

    public FacebookLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request,
        CancellationToken cancellationToken)
    {
       var token=  await _authService.FacebookLoginAsync(request.AuthToken,15);
       return new()
       {
           Token = token
       };
    }
}