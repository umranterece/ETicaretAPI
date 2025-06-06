using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler: IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    readonly IAuthService _authService;
    public GoogleLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var token= await _authService.GoogleLoginAsync(request.IdToken, 15);
        return new()
        {
            Token = token
        };
    }
}