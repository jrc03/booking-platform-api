using Application.Features.Users.DTOs;
using MediatR;
using System;

namespace Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(
        Guid UserId, 
        string FirstName, 
        string LastName
    ) : IRequest<UserResponseDto>;
}
