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
        public Booking() 
        { 
            // EF Core default constructor
            Dates = new DateRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        }
        public  Guid Id { get; init; } = Guid.NewGuid();
        public Guid PropertyId { get; private set; }
        public Guid GuestId { get; private set; }
        public DateRange Dates { get; private set; }
        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; } = BookingStatus.Confirmed;

        public static Booking Create(Guid propertyId, Guid guestId, DateRange dates, decimal pricePerNight)
        {
            var totalNights = (dates.End - dates.Start).Days;
            if (totalNights <= 0) totalNights = 1;
            
            return new Booking
            {
                PropertyId = propertyId,
                GuestId = guestId,
                Dates = dates,
                TotalPrice = totalNights * pricePerNight
            };
        }

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
            //TODO 
            // if (DateTime.UtcNow.Date <= Dates.End.Date)
            //     throw new InvalidOperationException("Booking can only be completed after the checkout date.");

            Status = BookingStatus.Completed;
        }
    }
}