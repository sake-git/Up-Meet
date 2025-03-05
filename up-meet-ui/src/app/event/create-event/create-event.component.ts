import { Component } from '@angular/core';
import { UserEvent } from '../../model/event';
import { User } from '../../model/user';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-event',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-event.component.html',
  styleUrl: './create-event.component.css',
})
export class CreateEventComponent {
  userEvent: UserEvent = {
    id: 0,
    name: '',
    location: '',
    eventDateTime: new Date(),
    imgUrl: '',
    description: '',
    price: 0,
    kidsAllowed: 0,
    duration: 0,
    createdBy: 0,
  };
  user: User | undefined;

  eventForm: FormGroup;

  errorMessage = '';

  constructor(private apiService: ApiService, private router: Router) {
    this.eventForm = new FormGroup({
      name: new FormControl('', Validators.required),
      location: new FormControl('', Validators.required),
      eventDateTime: new FormControl('', Validators.required),
      imgUrl: new FormControl(''),
      description: new FormControl('', [
        Validators.required,
        Validators.maxLength(500),
      ]),
      price: new FormControl(),
      kidsAllowed: new FormControl(true),
      duration: new FormControl('', Validators.required),
    });
  }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user')!); //history.state;
    console.log('Event Creation User info: ', this.user);
  }

  SaveEvent() {
    console.log('save event called');
    this.userEvent = this.eventForm.value;
    this.userEvent.createdBy = this.user!.id;
    this.apiService.createEvents(this.userEvent).subscribe({
      next: (data: any) => {
        this.router.navigateByUrl('home/list-event');
        console.log('Event added');
      },
      error: (error) => {
        this.errorMessage = error?.error;
        console.log('Error:', error);
      },
    });
  }

  Cancel() {
    this.router.navigateByUrl('home/list-event');
  }
}
