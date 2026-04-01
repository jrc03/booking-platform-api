using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties
{
    public record GetPropertyByIdQuery(Guid Id) : IRequest<PropertyResponseDto>
    {  }
}