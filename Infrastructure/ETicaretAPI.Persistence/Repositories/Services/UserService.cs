using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Persistence.Repositories.Services;

public class UserService: IUserService
{
    readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result= await _userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Username,
            Email = model.Email,
            NameSurname = model.NameSurname,
        },model.Password);

        CreateUserResponse response = new(){Succeeded =  result.Succeeded};
        
        if (result.Succeeded)
            response.Message="User created successfully";
        else
        {
            foreach (var error in result.Errors)
            {
                response.Message += $"{error.Code} - {error.Description}\n";
            }
        }
        return response;
    }
}