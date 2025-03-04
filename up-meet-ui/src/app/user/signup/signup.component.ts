import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { User } from '../../model/user';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-signup',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SignupComponent {
  user: User = {
    id: 0,
    name: '',
    loginId: '',
    email: '',
  };

  errorMessage = '';

  constructor(private apiService: ApiService, private router: Router) {}

  CreateUser() {
    console.log('Create user called', this.user);
    var message = this.apiService.createUser(this.user).subscribe({
      next: (data) => {
        console.log('User created successfully', data);
        this.router.navigateByUrl('/login');
      },
      error: (error) => {
        this.errorMessage = error.error;
        console.log('Error creating user', error);
      },
    });
  }

  Cancel() {
    this.router.navigateByUrl('/login');
  }
}
