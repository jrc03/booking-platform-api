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
        public Guid HostId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Location { get; private set; } = string.Empty;

        public decimal PricePerNight { get; private set; }
        public int Capacity { get; private set; }

        public static Property Create(Guid hostId, string title, string description, string location, decimal pricePerNight, int capacity)
        {
            return new Property
            {
                HostId = hostId,
                Title = title,
                Description = description,
                Location = location,
                PricePerNight = pricePerNight,
                Capacity = capacity
            };
        }

        public void UpdateDetails(string title, string description, string location, decimal pricePerNight, int capacity)
        {
            if (pricePerNight <= 0) throw new ArgumentException("Price must be greater than zero.");
            if (capacity <= 0) throw new ArgumentException("Capacity must be greater than zero.");

            Title = title;
            Description = description;
            Location = location;
            PricePerNight = pricePerNight;
            Capacity = capacity;
        }


        private readonly List<DateRange> _blockedDates = new();
        public IReadOnlyCollection<DateRange> BlockedDates => _blockedDates.AsReadOnly();

        private readonly List<string> _imageUrls = new();
        public IReadOnlyCollection<string> ImageUrls => _imageUrls.AsReadOnly();

        public void AddImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Image URL cannot be empty");
            _imageUrls.Add(url);
        }

        public void RemoveImage(string url)
        {
            _imageUrls.Remove(url);
        }

        public void UpdateImages(IEnumerable<string> urls)
        {
            _imageUrls.Clear();
            if (urls != null)
            {
                foreach (var url in urls)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        _imageUrls.Add(url);
                    }
                }
            }
        }


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