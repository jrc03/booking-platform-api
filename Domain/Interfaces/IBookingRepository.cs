using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByGuestAsync(Guid guestId); 
    Task<bool> HasOverlappingBookingsAsync(Guid propertyId, DateRange dates);
}