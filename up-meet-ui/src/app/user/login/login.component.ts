import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { User } from '../../model/user';
import { ApiService } from '../../services/api.service';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  user: User = {
    id: 0,
    name: '',
    loginId: '',
    password: '',
    email: '',
  };

  errorMessage = '';
  message = '';

  constructor(
    private apiService: ApiService,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {
    localStorage.removeItem('user');
    localStorage.removeItem('myToken');
    this.activeRoute.params.subscribe((params) => {
      this.errorMessage = params['error'];
      this.message = params['success'];
    });
  }

  userLogin(loginForm: NgForm) {
    console.log('User login called');
    this.apiService.getUser(this.user).subscribe({
      next: (data: any) => {
        this.user = data;
        localStorage.setItem('user', JSON.stringify(this.user));
        localStorage.setItem('myToken', this.user.token!);
        console.log('Data:', this.user);
        this.router.navigateByUrl('/home');
      },
      error: (error) => {
        console.log(error);
        this.errorMessage = error?.error;
        loginForm.reset();
      },
    });
  }

  Cancel(loginForm: NgForm) {
    console.log('cancel called');
    loginForm.reset();
    this.message = '';
    this.errorMessage = '';
  }
}
