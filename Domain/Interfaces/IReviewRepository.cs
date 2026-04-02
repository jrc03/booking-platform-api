using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByPropertyAsync(Guid propertyId);
        Task<IEnumerable<Review>> GetReviewsByGuestAsync(Guid guestId);
    }
}
