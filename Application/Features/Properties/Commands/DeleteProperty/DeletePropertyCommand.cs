using System;
using MediatR;

namespace Application.Features.Properties.Commands.DeleteProperty
{
    public record DeletePropertyCommand(Guid Id) : IRequest; // No retorna DTO, solo Unit/void
}