using Domain.Entities;

namespace Domain.Interfaces;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByGuestAsync(Guid guestId); 
}