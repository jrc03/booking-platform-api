using System.Text.RegularExpressions;
using MediatR;

namespace Application.Features.Properties.Commands
{
    public record PropertyResponseDto(Guid Id, string Title, string Description, string Location, int Capacity,         decimal PricePerNight);
    public record CreatePropertyCommand(
        Guid HostId,
        string Title,
        string Description,
        string Location,
        decimal PricePerNight,
        int Capacity
    ) : IRequest<PropertyResponseDto>;


}