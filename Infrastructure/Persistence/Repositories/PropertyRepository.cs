using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Property>> GetByHostIdAsync(Guid hostId)
        {
            return await _dbSet.Where(p => p.HostId == hostId).ToListAsync();
        }

        public async Task<IEnumerable<Property>> SearchAsync(string? location, DateTime? startDate, DateTime? endDate, int? minCapacity, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(p => p.Location.Contains(location));

            if (minCapacity.HasValue)
                query = query.Where(p => p.Capacity >= minCapacity.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.PricePerNight <= maxPrice.Value);

            var properties = await query.ToListAsync();

            if (properties.Count == 0)
                return new List<Property>();

            if (startDate.HasValue && endDate.HasValue)
            {
                var start = startDate.Value;
                var end = endDate.Value;

                var propertyIds = properties.Select(p => p.Id).ToList();

                var conflictingBookings = await _dbContext.Bookings
                    .Where(b => propertyIds.Contains(b.PropertyId) &&
                                b.Status == BookingStatus.Confirmed &&
                                b.Dates.Start < end && start < b.Dates.End)
                    .Select(b => b.PropertyId)
                    .ToListAsync();

                var conflictingPropertyIds = conflictingBookings.ToHashSet();

                properties = properties.Where(p =>
                    !conflictingPropertyIds.Contains(p.Id) &&
                    !p.BlockedDates.Any(b => b.Start < end && start < b.End)
                ).ToList();
            }

            return properties
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
