import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Appointment, AppointmentStatus } from 'src/app/core/models';
import { AppointmentsService } from 'src/app/core/services';
import { formatDate } from 'src/app/core/utils';

@Component({
  selector: 'app-appointment-form',
  templateUrl: './appointment-form.component.html',
})
export class AppointmentFormComponent implements OnInit {
  @Output() cancel = new EventEmitter<void>();
  @Output() appointmentAdded = new EventEmitter<Appointment>();

  @Input() appointment: Appointment | null = null;

  appointmentForm!: FormGroup;

  formAction = 'Add';

  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentsService
  ) {}

  ngOnInit(): void {
    this.formAction =
      (this.appointment?.appointmentId && this.appointment.appointmentId > 0)
        ? 'Edit'
        : 'Add';
    this.appointmentForm = this.fb.group({
      appointmentId: [this.appointment?.appointmentId ?? 0],
      title: [this.appointment?.title, Validators.required],
      date: [
        this.appointment?.date ? formatDate(this.appointment.date) : '',

        Validators.required,
      ],
      time: [this.appointment?.time, Validators.required],
      userId: [this.appointment?.userId ?? 0],
      status: [this.appointment?.status ?? AppointmentStatus.Pending],
    });
  }

  submit() {
    if (!this.appointmentForm.valid) {
      return;
    }

    const appointment = this.appointmentForm.value as Appointment;
    if (appointment.appointmentId && appointment.appointmentId > 0) {
      this.appointmentService
        .update(appointment)
        .subscribe(() => this.appointmentAdded.emit());
    } else {
      this.appointmentService
        .create(appointment)
        .subscribe(() => this.appointmentAdded.emit());
    }
    this.appointmentForm.reset();
  }

  onCancel() {
    this.cancel.emit();
  }
}
