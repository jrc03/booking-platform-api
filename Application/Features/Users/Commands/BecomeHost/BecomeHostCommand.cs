using MediatR;
using Application.Features.Users.DTOs;

namespace Application.Features.Users.Commands.BecomeHost
{
    public record BecomeHostCommand() : IRequest<UserResponseDto>;
}