import { Injectable } from '@angular/core';
import { User } from '../model/user';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEvent } from '../model/event';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  baseUrl = '';

  constructor(private http: HttpClient) {}

  createUser(user: User) {
    console.log('called Create user from api service', user);
    return this.http.post(`${this.baseUrl}/Users`, user);
  }

  getUser(user: User): Observable<User> {
    console.log('userlogin service called');
    return this.http.post<User>(`${this.baseUrl}/Users/Authenticate`, user);
  }
}
