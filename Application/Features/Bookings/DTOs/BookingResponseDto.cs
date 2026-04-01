using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Application.Features.Bookings.DTOs
{
    public record BookingResponseDto
  (
    Guid Id,
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice,
    string Status
  );
}