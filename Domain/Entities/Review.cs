using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public Review() { }

        public  Guid Id { get; init; } = Guid.NewGuid();
        public required Guid BookingId { get; init; }
        public required Guid GuestId { get; init; }
        public required Guid PropertyId { get; init; }

        public required int Rating { get; init; }
        public required string Comment { get; init; }
        public  DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public static Review Create(Guid bookingId, Guid guestId, Guid propertyId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be empty.", nameof(comment));

            return new Review
            {
                BookingId = bookingId,
                GuestId = guestId,
                PropertyId = propertyId,
                Rating = rating,
                Comment = comment,   
            };
        }

    }
}