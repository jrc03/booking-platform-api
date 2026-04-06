using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Users.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand
    (string Email, string Token) : IRequest<bool>;
}