using MediatR;
using System;

namespace Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : IRequest<bool>;
}
