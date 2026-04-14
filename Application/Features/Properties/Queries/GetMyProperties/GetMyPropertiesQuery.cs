using System.Collections.Generic;
using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Queries.GetMyProperties
{
    public record GetMyPropertiesQuery() : IRequest<IEnumerable<PropertyResponseDto>>;
}