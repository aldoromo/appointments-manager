import { Injectable } from '@angular/core';
import { User } from '../models';
import { Roles } from '../models/roles.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly STORAGE_KEY = 'currentUserId';
  private readonly STORAGE_USER = 'currentUser';

  setUserId(userId: number) {
    localStorage.setItem(this.STORAGE_KEY, userId.toString());
  }

  setUser(username: User) {
    localStorage.setItem(this.STORAGE_USER, JSON.stringify(username));
    this.setUserId(username.userId);
  }

  getUser(): User {
    const user = JSON.parse(
      localStorage.getItem(this.STORAGE_USER) ?? ''
    ) as User;
    return user;
  }

  isManager(): boolean {
    const user = this.getUser();
    return user?.role === Roles.Manager; // Assuming role 1 is Manager
  }

  getUserId(): number | null {
    const value = localStorage.getItem(this.STORAGE_KEY);
    return value ? parseInt(value, 10) : null;
  }

  clear() {
    localStorage.removeItem(this.STORAGE_KEY);
    localStorage.removeItem(this.STORAGE_USER);
  }

  isLoggedIn(): boolean {
    return this.getUserId() !== null;
  }
}
