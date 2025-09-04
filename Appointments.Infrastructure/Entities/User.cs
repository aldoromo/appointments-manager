

namespace Appointments.Infrastructure.Entities
{
    public enum UserRole { User = 0, Manager = 1 }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

        // Navigation property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
