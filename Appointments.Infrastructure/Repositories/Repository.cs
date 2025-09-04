using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories
{
   public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppointmentsDbContext _context;
        protected readonly DbSet<T> _set;

        public Repository(AppointmentsDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

        public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default) =>
            await _set.ToListAsync(cancellationToken);

        public virtual async Task AddAsync(T entity) => await _set.AddAsync(entity);

        public virtual void Update(T entity) => _set.Update(entity);

        public virtual void Delete(T entity) => _set.Remove(entity);
    }
}
