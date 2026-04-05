using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Users.DTOs;

namespace Application.Features.Users.Commands
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password, 
        bool IsHost = false
        ) : IRequest<UserResponseDto>;
}