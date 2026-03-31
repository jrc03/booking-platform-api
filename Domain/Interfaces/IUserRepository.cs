using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}