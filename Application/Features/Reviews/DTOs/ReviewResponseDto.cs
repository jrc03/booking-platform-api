using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Reviews.DTOs
{
    public record ReviewResponseDto
    (
        Guid Id,
        Guid BookingId,
        Guid GuestId,
        Guid PropertyId,
        int Rating,
        string Comment,
        DateTime CreatedAt
    );
}