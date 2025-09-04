using Appointments.Infrastructure.Data;
using Appointments.Infrastructure.Entities;
using Appointments.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Appointments.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppointmentsDbContext context) : base(context) { }

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
