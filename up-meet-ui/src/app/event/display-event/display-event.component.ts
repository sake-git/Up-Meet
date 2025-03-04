import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { DomSanitizer } from '@angular/platform-browser';
import { UserEvent } from '../../model/event';

@Component({
  selector: 'app-display-event',
  imports: [CommonModule],
  templateUrl: './display-event.component.html',
  styleUrl: './display-event.component.css',
})
export class DisplayEventComponent implements OnInit {
  userEvent: any;
  user: any;
  errorMessage = '';
  url: any;
  currentlink = '';
  constructor(
    private apiService: ApiService,
    public router: Router,
    private activeRoute: ActivatedRoute,
    private domSanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.user = history.state;
    this.currentlink = location.href;
    console.log('Event Display User info: ', this.user.id);
    let id = 0;
    this.activeRoute.params.subscribe((params) => {
      id = params['id'];
    });
    if (id) {
      this.apiService.getEvent(id, this.user.id).subscribe({
        next: (data: UserEvent) => {
          this.userEvent = data;
        },
        error: (error) => {
          this.errorMessage = error?.error;
        },
      });
    } else {
      this.errorMessage = 'Something went wrong';
    }
  }

  ngDoCheck() {
    console.log(this.userEvent);
    var url =
      'https://maps.google.com/maps?&q=' +
      this.userEvent.location +
      '&output=embed';
    this.url = this.domSanitizer.bypassSecurityTrustResourceUrl(url);
  }

  UpdateFavorite() {
    console.log('update called');
    if (this.userEvent.isFavourite) {
      this.apiService
        .removeFavoriteEvent(this.user.id, this.userEvent.id)
        .subscribe({
          next: (next) => {
            console.log('Removed from Favourites');
            this.userEvent.isFavourite = false;
          },
        });
    } else {
      this.apiService
        .addFavouriteEvent(this.user.id, this.userEvent.id)
        .subscribe({
          next: (data) => {
            console.log('Added Successfully');
            this.userEvent.isFavourite = true;
          },
          error: (error) => {
            console.log(error.error);
          },
        });
    }
  }
  DeleteEvent() {
    console.log('delete event called');
    if (!confirm('Are you sure to delete the event')) {
      return;
    }
    this.apiService.deleteEvent(this.user.id, this.userEvent.id).subscribe({
      next: (data) => {
        console.log('Event deleted');
        this.router.navigateByUrl('home/list-event', { state: this.user });
      },
      error: (error) => {
        console.log(error.error);
      },
    });
  }
  Cancel() {
    this.router.navigateByUrl('home/list-event', { state: this.user });
  }
}
