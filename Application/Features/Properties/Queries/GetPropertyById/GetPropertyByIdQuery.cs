using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Queries.GetPropertyById
{
    public record GetPropertyByIdQuery(Guid Id) : IRequest<PropertyResponseDto>
    {  }
}