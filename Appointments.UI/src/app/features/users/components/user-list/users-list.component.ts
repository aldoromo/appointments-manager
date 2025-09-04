import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models';
import { UsersService } from 'src/app/core/services';


@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
})
export class UsersListComponent implements OnInit {
  users: User[] = [];
  displayForm = false;
  selectedUser: User | null = null;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.usersService.getAll().subscribe((data) => (this.users = data));
  }

  addUser() {
    this.selectedUser = { userId: 0, username: '', role: 0 };
    this.displayForm = true;
  }

  editUser(user: User) {
    this.selectedUser = { ...user };
    this.displayForm = true;
  }

  deleteUser(user: User) {
    if (confirm(`Delete user ${user.username}?`)) {
      this.usersService.delete(user.userId).subscribe(() => this.loadUsers());
    }
  }

  onFormSave() {
    this.displayForm = false;
    this.loadUsers();
  }

  onFormCancel() {
    this.displayForm = false;
  }
}
