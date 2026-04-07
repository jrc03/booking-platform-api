using System.Text.RegularExpressions;
using MediatR;
using Application.Features.Properties.DTOs;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public record CreatePropertyCommand(
        string Title,
        string Description,
        string Location,
        decimal PricePerNight,
        int Capacity
    ) : IRequest<PropertyResponseDto>;


}