import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { AuthService } from 'src/app/core/services';
import { MenubarModule } from 'primeng/menubar';
import { SidebarModule } from 'primeng/sidebar';
import { CardModule } from 'primeng/card';
import { PrimeNGModules } from './shared';
import { CommonModule } from '@angular/common';
import { User } from './core/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  
  constructor(private authService: AuthService, private router: Router) {}

  users = [
    { name: 'John Doe', role: 'User' },
    { name: 'Alice Manager', role: 'Manager' },
  ];

  sidebarVisible = false;
  usernameInput: string = '';

  pageTitle = 'Appointment Manager';
  currentUser: string | null = 'Alice Manager';

  menuItems = [
    { label: 'Users', icon: 'pi pi-users', routerLink: '/users' },
    {
      label: 'Appointments',
      icon: 'pi pi-calendar',
      routerLink: '/appointments',
    },
  ];

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }

  isLoggedIn(): boolean {
    const isLoggedIn = this.authService.isLoggedIn();
    return isLoggedIn;;
  }

  onLoginSuccess($event: User) {
    this.authService.setUserId($event.userId);
    this.currentUser = $event.username;
    this.router.navigate(['/appointments']);

}

  logout() {
    this.authService.clear();
    this.router.navigate(['/login']);
  }
}
