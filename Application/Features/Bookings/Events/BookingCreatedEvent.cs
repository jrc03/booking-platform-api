using MediatR;
using System;

namespace Application.Features.Bookings.Events;

public record BookingCreatedEvent(Guid BookingId, Guid PropertyId, Guid GuestId, decimal TotalPrice) : INotification;