using System;
using System.Collections.Generic;

namespace Application.Features.Properties.DTOs
{
    public record PropertyResponseDto(Guid Id, string Title, string Description, string Location, int Capacity, decimal PricePerNight, IEnumerable<string> ImageUrls);
}