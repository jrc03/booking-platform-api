using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
