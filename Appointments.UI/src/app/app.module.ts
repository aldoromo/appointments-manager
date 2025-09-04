import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

// PrimeNG modules
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { PrimeNGModules } from './shared';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { AppRoutingModule, routes } from './app.routes';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthModule } from './features/auth/auth.module';
import { AppointmentsService, UsersService } from './core/services';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MainContainerComponent } from './features/containers';
import { AuthInterceptor } from './core/interceptors';

@NgModule({
  declarations: [AppComponent, MainContainerComponent], //
  imports: [
    BrowserModule,
    //RouterModule.forRoot(routes),
    AppRoutingModule, 
    CommonModule,
    RouterModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AuthModule,
    ...PrimeNGModules,
  ],
  providers: [
     { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    UsersService, AppointmentsService],
  //schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent], //
})
export class AppModule {}
