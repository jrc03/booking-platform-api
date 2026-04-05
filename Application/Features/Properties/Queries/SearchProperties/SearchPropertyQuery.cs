using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using MediatR;

namespace Application.Features.Properties.Queries.SearchProperties
{
    public record SearchPropertyQuery
    (
    string? Location,
    DateTime? StartDate,
    DateTime? EndDate,
    int? MinCapacity,
    decimal? MaxPrice,
    int PageNumber = 1,
    int PageSize = 20
    ) : IRequest<IEnumerable<PropertyResponseDto>>;
}