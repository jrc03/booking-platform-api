using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification
    {
        public Notification() { }

        public Guid Id { get; init; } = Guid.NewGuid();
        public required Guid UserId { get; init; }
        public required string Message { get; init; }

        public bool IsRead { get; private set; } = false;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        public static Notification Create(Guid userId, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            return new Notification
            {
                UserId = userId,
                Message = message
            };
        }
        public void MaskAsRead()
        {
            IsRead = true;
        }
    }
}