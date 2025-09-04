import { Component, EventEmitter, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';
import { UsersService, AuthService } from 'src/app/core/services';
import { Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  signUp() {
    this.createNewUser = true;
  }
  @Output()
  loginSuccess = new EventEmitter<User>();
  users: User[] = [];
  username: string | null = null;
  createNewUser = false;
  message = '';

  constructor(
    private usersService: UsersService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  login() {
    if (!this.username) {
      return;
    }

    this.usersService.getByUsername(this.username).subscribe({
      next: (user) => {
        this.loginSuccess.emit(user);
        this.authService.setUser(user);
        this.router.navigate(['dashboard', 'appointments']);
      },
      error: (err) => {
        this.message = 'User not found';
      },
    });
  }

  save() {
    this.createNewUser = false;
  }

  cancel() {
    this.createNewUser = false;
  }

  form!: FormGroup;
  roles = [
    { label: 'User', value: 0 },
    { label: 'Manager', value: 1 },
  ];
}
