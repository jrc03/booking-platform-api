using System.Collections.Generic;
using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Queries.GetAllProperties
{
    public record GetAllPropertiesQuery : IRequest<IEnumerable<PropertyResponseDto>>;
}