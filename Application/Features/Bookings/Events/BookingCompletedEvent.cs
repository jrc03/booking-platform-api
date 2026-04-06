using MediatR;
using System;

namespace Application.Features.Bookings.Events;

public record BookingCompletedEvent(Guid BookingId, Guid PropertyId, Guid GuestId) : INotification;