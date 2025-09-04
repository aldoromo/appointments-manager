export interface Appointment {
  appointmentId: number;
  title: string;
  date: string;
  time: string;
  userId: number;
  username: string;
  status: number; //Pending = 0, Approved = 1, Canceled = 2
}
