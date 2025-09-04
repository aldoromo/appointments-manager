import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    if (this.authService.isLoggedIn()) {
      //const isManager = this.authService.isManager();
      // const isManager = true;
      // if (isManager) {
      //   return this.router.parseUrl('/dashaboard/appointments');
      // }
      return true;
    }
    return this.router.parseUrl('/login');
  }
}
