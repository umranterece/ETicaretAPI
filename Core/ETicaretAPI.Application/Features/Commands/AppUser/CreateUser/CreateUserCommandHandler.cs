using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        
       CreateUserResponse response= await _userService.CreateAsync(new()
        {
            NameSurname = request.NameSurname,
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            PasswordConfirm = request.PasswordConfirm
        } );
        return new()
        {
            Message = response.Message,
            Succeeded = response.Succeeded
        };
    }
}