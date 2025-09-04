import {
  CUSTOM_ELEMENTS_SCHEMA,
  NgModule,
  NO_ERRORS_SCHEMA,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { LoginComponent } from './components';

// PrimeNG
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { AuthRoutingModule } from './auth-routing.module';
import { PrimeNGModules } from 'src/app/shared';
import { UsersModule } from '../users/users.module';
import { UserFormComponent } from '../users/components';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AuthRoutingModule,
    DropdownModule,
    ButtonModule,

    UsersModule,
    


    ...PrimeNGModules,
  ],
  exports: [LoginComponent],
})
export class AuthModule {}
