using System;
using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public record UpdatePropertyCommand(
        Guid Id, // El ID de la propiedad a modificar
        string Title,
        string Description,
        string Location,
        decimal PricePerNight,
        int Capacity
    ) : IRequest<PropertyResponseDto>;
}