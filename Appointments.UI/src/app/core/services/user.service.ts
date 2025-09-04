import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { from, Observable, of } from 'rxjs';
import { User } from 'src/app/core/models';
import { Roles } from '../models/roles.model';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<User[]> {
    // return of([
    //   {
    //     id: 1,
    //     name: 'user',
    //     role: Roles.User, // 0 = User, 1 = Manager
    //   },
    //   {
    //     id: 2,
    //     name: 'manager',
    //     role: Roles.Manager, // 0 = User, 1 = Manager
    //   },
    // ]);
     return this.http.get<User[]>(this.apiUrl);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  getByUsername(username: string): Observable<User> {
    // return of({ id: 1, name: 'user', role: Roles.User }); //1 means
    return this.http.get<User>(`${this.apiUrl}/${username}`);
  }

  create(user: Partial<User>): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  update(user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${user.userId}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
