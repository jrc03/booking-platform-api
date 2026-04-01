using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Users.Commands
{
    public record UserResponseDto(Guid Id, string FirstName, string LastName, string Email);
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password, // TODO Lo ideal luego es hacer un hash a la DB
        bool IsHost) : IRequest<UserResponseDto>;
}