using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Interfaces;

namespace Appointments.Infrastructure.Repositories
{
   public class UnitOfWork : IUnitOfWork
    {
        private readonly AppointmentsDbContext _context;
        public UnitOfWork(AppointmentsDbContext context)
        {
            _context = context;
            Appointments = new Repositories.AppointmentRepository(_context);
            Users = new Repositories.UserRepository(_context);
        }

        public IAppointmentRepository Appointments { get; private set; }
        public IUserRepository Users { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
