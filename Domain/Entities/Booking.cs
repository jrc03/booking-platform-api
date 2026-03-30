using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Booking
    {
        public Booking() { }
        public  Guid Id { get; init; } = Guid.NewGuid();
        public required Guid PropertyId { get; init; }
        public required Guid GuestId { get; init; }

        public required DateRange Dates { get; init; }
        public required decimal TotalPrice { get; init; }

        public BookingStatus Status { get; private set; } = BookingStatus.Confirmed;

        // Concurrency Token to prevent Double-Bookings (EF Core Optimistic Concurrency)
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();


        public void Cancel()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings  can be cancelled");

            Status = BookingStatus.Cancelled;
        }
        public void Complete()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings can be completed.");

            if (DateTime.UtcNow.Date <= Dates.End.Date)
                throw new InvalidOperationException("Booking can only be completed after the checkout date.");

            Status = BookingStatus.Completed;
        }
    }
}