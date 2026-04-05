using Domain.Entities;

namespace Domain.Interfaces;

public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<IEnumerable<Property>> SearchAsync(
     string? location,
     DateTime? startDate,
     DateTime? endDate,
     int? minCapacity,
     decimal? maxPrice,
     int pageNumber,
     int pageSize);
}