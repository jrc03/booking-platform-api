using System;
using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public record UpdatePropertyCommand(
        Guid Id, // The ID of the property to modify
        string Title,
        string Description,
        string Location,
        decimal PricePerNight,
        int Capacity
    ) : IRequest<PropertyResponseDto>;
}
