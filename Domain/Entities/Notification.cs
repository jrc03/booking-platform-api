using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification
    {
        public Notification() { }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public string Message { get; private set; } = string.Empty;

        public bool IsRead { get; private set; } = false;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

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
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}