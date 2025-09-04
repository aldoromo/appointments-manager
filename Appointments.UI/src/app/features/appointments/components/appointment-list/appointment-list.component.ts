import { Component, OnInit } from '@angular/core';
import { Appointment, AppointmentStatus } from 'src/app/core/models';
import { AppointmentsService, AuthService } from 'src/app/core/services';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss'],
})
export class AppointmentListComponent implements OnInit {
  readonly Canceled = AppointmentStatus.Canceled;
  readonly Approved = AppointmentStatus.Approved;
  readonly Pending = AppointmentStatus.Pending;
  constructor(
    private appointmentsService: AppointmentsService,
    private authService: AuthService
  ) {}

  isManager: any;

  displayForm = false;
  selectedAppointment: Appointment | null = null;
  groupedAppointments: { date: string; items: Appointment[] }[] = [];

  appointments: Appointment[] = [];

  ngOnInit(): void {
    this.isManager = this.authService.isManager();
    this.appointments = [];
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentsService.getAll().subscribe((data) => {
      this.appointments = data;
      this.groupByDate();
    });
  }

  addAppointment() {
    this.selectedAppointment = {
      appointmentId: 0,
      title: '',
      date: '',
      time: '09:00',
      userId: 0,
      status: AppointmentStatus.Pending,
      username: '',
    };
    this.displayForm = true;
  }

  editAppointment(appointment: Appointment) {
    this.selectedAppointment = { ...appointment };
    this.displayForm = true;
  }

  deleteAppointment(appointment: Appointment) {
    if (!this.selectedAppointment) return;
    if (confirm(`Delete appointment ${appointment.title}?`)) {
      this.appointmentsService
        .delete(this.selectedAppointment.appointmentId)
        .subscribe(() => this.loadAppointments());
    }
  }

  onFormSave() {
    this.displayForm = false;
    this.loadAppointments();
  }

  onFormCancel() {
    this.displayForm = false;
  }

  private groupByDate() {
    const groups: { [key: string]: Appointment[] } = {};
    for (const appt of this.appointments) {
      if (!groups[appt.date]) {
        groups[appt.date] = [];
      }
      groups[appt.date].push(appt);
    }
    this.groupedAppointments = Object.keys(groups).map((date) => ({
      date,
      items: groups[date],
    }));
  }

  edit(appt: Appointment) {
    this.selectedAppointment = { ...appt };
    this.displayForm = true;
  }

  delete(appt: Appointment) {
    this.selectedAppointment = { ...appt };
    if (
      confirm(
        `Are you sure you want to delete this appointment, ${this.selectedAppointment.title}?`
      )
    ) {
      this.appointmentsService
        .delete(this.selectedAppointment.appointmentId)
        .subscribe(() => this.loadAppointments());
    }
  }

  cancel(appt: Appointment) {
     this.selectedAppointment = { ...appt };
     if (
       confirm(
         `Are you sure you want to CANCEL this appointment, ${this.selectedAppointment.title}?`
       )
     ) {
       this.appointmentsService
         .cancel(this.selectedAppointment.appointmentId)
         .subscribe(() => this.loadAppointments());
     }
  }

  approve(appt: Appointment) {
    appt.status = AppointmentStatus.Approved;
    this.selectedAppointment = { ...appt };
    if (
      confirm(
        `Are you sure you want to APPROVE this appointment, ${this.selectedAppointment.title}?`
      )
    ) {
      this.appointmentsService
        .approve(this.selectedAppointment.appointmentId)
        .subscribe(() => this.loadAppointments());
    }
  }

  onAppointmentAdded($event: Appointment) {
    this.displayForm = false;
    this.loadAppointments();
  }

  getTitle(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'Pending';
      case AppointmentStatus.Approved:
        return 'Approved';
      case AppointmentStatus.Canceled:
        return 'Canceled';
      default:
        return 'Unknown';
    }
  }
}
