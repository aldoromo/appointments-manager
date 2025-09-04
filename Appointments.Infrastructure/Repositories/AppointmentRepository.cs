using System.Globalization;
using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Entities;
using Appointments.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories
{
   public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentsDbContext context) : base(context) { }

        public override async Task<IEnumerable<Appointment>> ListAsync(CancellationToken cancellationToken = default) =>
             await _context.Appointments
                .Include(x=> x.User)
                .ToListAsync();

        public async Task<IEnumerable<Appointment>> ListAsync(string orderBy = null) 
        {
            var query = _context.Appointments
                .Include(x => x.User)
                .AsQueryable();
            
            query = orderBy.ToLower() switch
            {
                "username" => query.OrderBy(a => a.User.Username),
                "date" => query.OrderBy(a => a.Date),
                _ => query
            };

            var appointments = await query.ToListAsync();
            return appointments;
        }

        public async Task<IEnumerable<Appointment>> FindByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Appointments
                .Where(a => a.Date >= from && a.Date <= to)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> FindByUserIdAsync(int userId)
        {
            return await _context.Appointments
                .Include( x=> x.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Appointments.AnyAsync(a => a.AppointmentId == id);

        public override async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id);
        }
    }
}
