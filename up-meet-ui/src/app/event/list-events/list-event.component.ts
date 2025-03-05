import { Component, OnInit } from '@angular/core';
import { User } from '../../model/user';
import { UserEvent } from '../../model/event';
import { ApiService } from '../../services/api.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-event',
  imports: [CommonModule, RouterLink],
  templateUrl: './list-event.component.html',
  styleUrl: './list-event.component.css',
})
export class ListEventComponent implements OnInit {
  user: User = {
    id: 0,
    name: '',
    loginId: '',
    password: '',
    email: '',
  };

  events: UserEvent[] = [];

  errorMessage = '';

  constructor(private apiService: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user')!); //history.state;

    console.log('Event Creation User info: ', this.user);

    this.apiService.getEvents(0).subscribe({
      next: (data: UserEvent[]) => {
        this.events = data;
      },
      error: (error) => {
        this.errorMessage = error?.error;
      },
    });
  }

  Cancel() {
    this.router.navigateByUrl('/login');
  }
}
