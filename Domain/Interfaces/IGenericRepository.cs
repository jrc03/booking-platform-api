namespace Domain.Interfaces;


public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    void Add(T entity);
   void Update(T entity);
    void Delete(T entity); 
}