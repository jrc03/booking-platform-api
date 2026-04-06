using MediatR;
using System;

namespace Application.Features.Bookings.Events;

public record BookingCancelledEvent(Guid BookingId, Guid PropertyId, Guid GuestId) : INotification;