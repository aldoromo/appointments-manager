import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// PrimeNG
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { PrimeNGModules } from 'src/app/shared';
import { HttpClientModule } from '@angular/common/http';
import { AppointmentListComponent } from './components/appointment-list/appointment-list.component';
import { appointmentsRoutingModule,  } from './appointments-routing.module';
import { AppointmentFormComponent } from './components/appointment-form/appointment-form.component';

@NgModule({
  declarations: [AppointmentListComponent, AppointmentFormComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    TableModule,
    ButtonModule,
    DialogModule,
    InputTextModule,
    DropdownModule,
    appointmentsRoutingModule,
  
    ...PrimeNGModules
  ],
})
export class AppointmentsModule {}
