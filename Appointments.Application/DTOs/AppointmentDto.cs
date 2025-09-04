using Appointments.Infrastructure.Entities;

namespace Appointments.Application.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; } = (int)AppointmentStatus.Pending;

        // Navigation property
        public int UserId { get; set; }
        public string? Username { get; set; }
    }
}
