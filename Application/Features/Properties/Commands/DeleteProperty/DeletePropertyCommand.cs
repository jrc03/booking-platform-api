using System;
using MediatR;

namespace Application.Features.Properties.Commands.DeleteProperty
{
    public record DeletePropertyCommand(Guid Id) : IRequest; // Doesn't return DTO, only Unit/void
}
