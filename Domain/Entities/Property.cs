using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Property
    {
        public Property() { }
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid HostId { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }

        public required decimal PricePerNight { get; init; }
        public required int Capacity { get; init; }


        private readonly List<DateRange> _blockedDates = new();

        public IReadOnlyCollection<DateRange> BlockedDates => _blockedDates.AsReadOnly();


        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();
        public DateTime LastBookedAt { get; private set; } = DateTime.UtcNow;

        public void BlockDateRange(DateRange range)
        {
            foreach (var blocked in _blockedDates)
            {
                if (blocked.Overlaps(range))
                    throw new InvalidOperationException("The property is already blocked for the selected days");
            }
            _blockedDates.Add(range);
        }
        public void RecordBooking()
        {
            LastBookedAt = DateTime.UtcNow;
        }

    }
}