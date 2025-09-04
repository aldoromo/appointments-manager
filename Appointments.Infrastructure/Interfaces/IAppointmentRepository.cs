using Appointments.Infrastructure.Entities;


namespace Appointments.Infrastructure.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> ListAsync(string orderBy = null);
        Task<IEnumerable<Appointment>> FindByDateRangeAsync(DateTime from, DateTime to);
        Task<IEnumerable<Appointment>> FindByUserIdAsync(int userId);
        Task<bool> ExistsAsync(int id);
    }
}
