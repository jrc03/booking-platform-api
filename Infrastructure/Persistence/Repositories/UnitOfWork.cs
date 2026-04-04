using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Aquí es donde realmente se dispara el SQL hacia la base de datos
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
