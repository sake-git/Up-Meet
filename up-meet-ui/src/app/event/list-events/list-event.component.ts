import { Component, OnInit } from '@angular/core';
import { User } from '../../model/user';
import { UserEvent } from '../../model/event';
import { ApiService } from '../../services/api.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { isValidDate } from 'rxjs/internal/util/isDate';
import moment from 'moment';

@Component({
  selector: 'app-list-event',
  imports: [CommonModule, RouterLink, FormsModule],
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
  eventDate = moment(new Date()).format('MM/DD/YYYY');
  location = '';
  isDateValid = true;

  constructor(private apiService: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user')!); //history.state;

    console.log('Event Creation User info: ', this.user);
    this.getEvents();
  }

  getEvents() {
    console.log('Event Creation User info: ', this.user);

    if (this.isDateValid) {
      this.apiService
        .getEvents(this.location, encodeURIComponent(this.eventDate))
        .subscribe({
          next: (data: UserEvent[]) => {
            this.events = data;
          },
          error: (error) => {
            console.log(error?.error);
          },
        });
    }
  }

  validateDate() {
    this.isDateValid = moment(this.eventDate, 'MM/DD/YYYY', true).isValid();
  }

  Cancel() {
    this.router.navigateByUrl('/login');
  }
}
