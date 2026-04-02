using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Users.DTOs;
using MediatR;

namespace Application.Features.Users.Queries.LoginUser
{
    public record LoginUserQuery(string Email, string Password) : IRequest<AuthenticationResponseDto>;
}