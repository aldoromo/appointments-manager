import { Component, OnInit } from '@angular/core';
import { Router, } from '@angular/router';
import { User } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services';


@Component({
  selector: 'app-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.scss'],
})
export class MainContainerComponent implements OnInit {
  
  constructor(private authService: AuthService, private router: Router) {}
    
  
  ngOnInit(): void {
     this.currentUser = this.authService.getUser().username;
     this.isManager = this.authService.isManager();
    }

   
  sidebarVisible = false;
  usernameInput: string = '';

  pageTitle = 'Appointment Manager';
  currentUser: string | null = 'Alice Manager';
  isManager = false;

  menuItems = [
    { label: 'Users', icon: 'pi pi-users', routerLink: '/dashboard/users' },
    {
      label: 'Appointments',
      icon: 'pi pi-calendar',
      routerLink: '/dashboard/appointments',
    },
  ];

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }

  isLoggedIn(): boolean {
    const isLoggedIn = this.authService.isLoggedIn();
    return isLoggedIn;;
  }

  logout() {
    this.authService.clear();
    this.router.navigate(['/login']);
  }
}
