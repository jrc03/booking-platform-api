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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByGuestAsync(Guid guestId)
        {
            return await _dbSet
                .Where(r => r.GuestId == guestId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyAsync(Guid propertyId)
        {
            return await _dbSet
                .Where(r => r.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}
