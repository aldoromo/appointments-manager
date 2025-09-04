import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, of, tap } from 'rxjs';
import { Appointment } from 'src/app/core/models';

@Injectable({
  providedIn: 'root',
})
export class AppointmentsService {
  private apiUrl = `${environment.apiUrl}/appointments`;

  constructor(private http: HttpClient) {}

  private withUserHeader(userId: number) {
    return {
      headers: new HttpHeaders({
        'X-User-Id': userId.toString(),
      }),
    };
  }

  getAll(): Observable<Appointment[]> {
  //   return of([
  //   {
  //     id: 1,
  //     title: 'Dental Checkup',
  //     date: '2025-09-03',
  //     time: '09:00',
  //     userId: 1,
  //   },
  //   {
  //     id: 2,
  //     title: 'Team Meeting',
  //     date: '2025-09-03',
  //     time: '14:00',
  //     userId: 2,
  //   },
  //   {
  //     id: 3,
  //     title: 'Lunch with Maria',
  //     date: '2025-09-04',
  //     time: '12:00',
  //     userId: 3,
  //   },
  // ] as Appointment[]);
    return this.http.get<Appointment[]>(
      this.apiUrl,
    )
  }

  getById(id: number): Observable<Appointment> {
    return this.http.get<Appointment>(
      `${this.apiUrl}/${id}`,
    );
  }

  create(
    appointment: Partial<Appointment>,
  ): Observable<Appointment> {
    return this.http.post<Appointment>(
      this.apiUrl,
      appointment,
    );
  }

  update(appointment: Appointment,): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${appointment.appointmentId}`,
      appointment,
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
    );
  }

  approve(id: number): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}/approve`,
      {},
    );
  }

  cancel(id: number): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}/cancel`,
      {},
    );
  }
}
