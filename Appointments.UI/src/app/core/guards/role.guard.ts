import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    const isManager = this.authService.isManager();

    if (isManager) {
      return true;
    }
    this.router.navigate(['/dashboard/appointments']);
    return false;
  }
}
