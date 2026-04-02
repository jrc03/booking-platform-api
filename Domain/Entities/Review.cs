using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public Review() { }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid BookingId { get; private set; }
        public Guid GuestId { get; private set; }
        public Guid PropertyId { get; private set; }

        public int Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

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

        public void UpdateReview(int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be empty.", nameof(comment));

            Rating = rating;
            Comment = comment;
        }

    }
}