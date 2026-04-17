using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain.ValueObjects;
using Domain.Enums;

namespace Infrastructure.Persistence.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Booking>> GetBookingsByGuestAsync(Guid guestId)
        {
            return await _dbSet
                .Where(b => b.GuestId == guestId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPropertyAsync(Guid propertyId)
        {
            return await _dbSet
                .Where(b => b.PropertyId == propertyId && b.Status == BookingStatus.Confirmed)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingBookingsAsync(Guid propertyId, DateRange dates)
        {
            return await _dbSet.AnyAsync(b => 
                b.PropertyId == propertyId &&
                b.Status == BookingStatus.Confirmed &&
                b.Dates.Start < dates.End && 
                dates.Start < b.Dates.End);
        }
    }
}
