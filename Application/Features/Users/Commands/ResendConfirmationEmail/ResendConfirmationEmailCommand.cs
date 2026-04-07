using MediatR;

namespace Application.Features.Users.Commands.ResendConfirmationEmail
{
    public record ResendConfirmationEmailCommand(string Email) : IRequest;
}