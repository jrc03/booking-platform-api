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

        public async Task<IEnumerable<Property>> GetAvailablePropertiesAsync(string location, DateTime start, DateTime end, int capacity)
        {
           
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(p => p.Location.Contains(location));
            

            var properties = await query
                .Where(p => p.Capacity >= capacity)
                .ToListAsync();

            if (properties.Count == 0)
                return properties;

            var propertyIds = properties.Select(p => p.Id).ToList();
            
            var conflictingBookings = await _dbContext.Bookings
                .Where(b => propertyIds.Contains(b.PropertyId) && 
                            b.Status == BookingStatus.Confirmed &&
                            b.Dates.Start < end && start < b.Dates.End)
                .Select(b => b.PropertyId)
                .ToListAsync();

            var conflictingPropertyIds = conflictingBookings.ToHashSet();

            var availableProperties = properties.Where(p => 
                !conflictingPropertyIds.Contains(p.Id) &&
                !p.BlockedDates.Any(b => b.Start < end && start < b.End)
            ).ToList();

            return availableProperties;
        }
    }
}
