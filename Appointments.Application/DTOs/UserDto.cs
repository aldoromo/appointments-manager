using Appointments.Infrastructure.Entities;

namespace Appointments.Application.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Role { get; set; } = (int)UserRole.User;
    }
}
