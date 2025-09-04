using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Infrastructure.Entities
{
    public enum AppointmentStatus { Pending = 0, Approved = 1, Canceled = 2 }
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Time{ get; set; }
        public string Title { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        // Navigation property
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
