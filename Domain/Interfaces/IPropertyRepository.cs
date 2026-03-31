using Domain.Entities;

namespace Domain.Interfaces;

public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<IEnumerable<Property>> GetAvailablePropertiesAsync(string location, DateTime start, DateTime end, int capacity);
}