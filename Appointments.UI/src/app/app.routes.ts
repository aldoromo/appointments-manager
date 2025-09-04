import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards';
import { MainContainerComponent } from './features/containers';
import { RoleGuard } from './core/guards';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadChildren: () =>
      import('./features/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'dashboard',
    component: MainContainerComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'users',
        loadChildren: () =>
          import('./features/users/users.module').then((m) => m.UsersModule),
        canActivate: [RoleGuard], //
      },
      {
        path: 'appointments',
        loadChildren: () =>
          import('./features/appointments/appointments.module').then(
            (m) => m.AppointmentsModule
          ),
      },
       { path: '**', redirectTo: '/dashboard/appointments' },
    ],
  },
  { path: '**', redirectTo: '/dashboard/appointments' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
