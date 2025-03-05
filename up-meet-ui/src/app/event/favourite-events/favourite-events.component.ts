import { Component, OnInit } from '@angular/core';
import { User } from '../../model/user';
import { UserEvent } from '../../model/event';
import { ApiService } from '../../services/api.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-favourite-events',
  imports: [CommonModule, RouterLink],
  templateUrl: './favourite-events.component.html',
  styleUrl: './favourite-events.component.css',
})
export class FavouriteEventsComponent implements OnInit {
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
    this.user = JSON.parse(localStorage.getItem('user')!);
    console.log('Event List Favourite User info: ', this.user);

    this.apiService.getFavouriteEvents(this.user.id).subscribe({
      next: (data: UserEvent[]) => {
        this.events = data;
        console.log('Data:', data);
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
