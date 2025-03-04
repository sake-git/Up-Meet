import { Injectable } from '@angular/core';
import { User } from '../model/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEvent } from '../model/event';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  baseUrl = 'https://localhost:7012/api';

  constructor(private http: HttpClient) {}

  //Create User
  createUser(user: User) {
    console.log('called Create user from api service', user);
    return this.http.post(`${this.baseUrl}/Users`, user);
  }

  //Authenticate user and get user information
  getUser(user: User): Observable<User> {
    console.log('userlogin service called');
    return this.http.post<User>(`${this.baseUrl}/Users/Authenticate`, user);
  }

  //Create Event
  createEvents(userEvent: UserEvent) {
    console.log('called Create Event from api service', userEvent);
    return this.http.post(`${this.baseUrl}/Events`, userEvent);
  }

  //Get event details for display
  getEvent(id: number, userId: number): Observable<UserEvent> {
    console.log('Get events service called' + id + userId);
    return this.http.get<UserEvent>(`${this.baseUrl}/Events/${id}/${userId}`);
  }

  //Get Event list
  getEvents(id: number): Observable<UserEvent[]> {
    console.log('Get events service called');
    return this.http.get<UserEvent[]>(`${this.baseUrl}/Events`);
  }

  //Get favourite events for given user
  getFavouriteEvents(id: number): Observable<UserEvent[]> {
    console.log('Get events service called', id);
    return this.http.get<UserEvent[]>(
      `${this.baseUrl}/FavouriteEvents/list/${id}`
    );
  }

  //Add to favourites for user
  addFavouriteEvent(userid: number, eventid: number) {
    console.log('Add Favourite event service called');
    return this.http.post<UserEvent[]>(`${this.baseUrl}/FavouriteEvents`, {
      eventId: eventid,
      userId: userid,
    });
  }

  //Remove from favourites
  removeFavoriteEvent(userId: number, eventId: number) {
    console.log('Remove Favourite event service called');
    return this.http.delete(
      `${this.baseUrl}/FavouriteEvents/${userId}/${eventId}`
    );
  }

  //Delete a given user
  deleteEvent(userId: number, eventId: number) {
    console.log('Delete event service called');
    return this.http.delete(`${this.baseUrl}/Events/${userId}/${eventId}`);
  }
}
