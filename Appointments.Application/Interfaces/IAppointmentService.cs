using Appointments.Application.DTOs;


namespace Appointments.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync(string orderBy=null);
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(int userId, string orderBy=null);
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<int> CreateAsync(AppointmentDto dto, int userId);
        Task UpdateAsync(AppointmentDto dto, int userId);
        Task ApproveAsync(int id, int userId);  // <- Manager
        Task CancelAsync(int id, int userId);   // <- Manager
        Task DeleteAsync(int id, int userId);
    }
}
