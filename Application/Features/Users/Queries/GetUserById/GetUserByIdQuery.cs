using Application.Features.Users.DTOs;
using MediatR;
using System;

namespace Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserResponseDto>;
}
