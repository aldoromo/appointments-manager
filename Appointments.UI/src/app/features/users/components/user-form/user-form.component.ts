import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsersService } from 'src/app/core/services';
import { User } from 'src/app/core/models';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  @Input() user: User | null = null;
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  form!: FormGroup;
  roles = [
    { label: 'User', value: 0 },
    { label: 'Manager', value: 1 },
  ];

  constructor(private fb: FormBuilder, private usersService: UsersService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      userId: [this.user?.userId ?? 0],
      username: [this.user?.username ?? '', Validators.required],
      role: [this.user?.role ?? 0, Validators.required],
    });
  }

  onSubmit() {
    const user = this.form.value as User;
    if (user.userId && user.userId > 0) {
      this.usersService.update(user).subscribe(() => this.save.emit());
    } else {
      this.usersService.create(user).subscribe(() => this.save.emit());
    }
  }

  onCancel() {
    this.cancel.emit();
  }
}
