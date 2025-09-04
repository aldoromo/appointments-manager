using System;

namespace Appointments.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IAppointmentRepository Appointments { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
