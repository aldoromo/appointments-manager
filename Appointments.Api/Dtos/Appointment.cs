namespace Appointments.Models
{
    public class Appointment
    {
        
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


    }
}
