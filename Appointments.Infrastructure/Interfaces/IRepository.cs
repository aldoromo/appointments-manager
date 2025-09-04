 

namespace Appointments.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
