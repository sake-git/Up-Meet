import { Routes } from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { SignupComponent } from './user/signup/signup.component';
import { authGuard } from './auth.guard';
import { ListEventComponent } from './event/list-events/list-event.component';
import { DisplayEventComponent } from './event/display-event/display-event.component';
import { FavouriteEventsComponent } from './event/favourite-events/favourite-events.component';
import { CreateEventComponent } from './event/create-event/create-event.component';
import { HomeComponent } from './event/home/home.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'list-event',
        component: ListEventComponent,
        canActivate: [authGuard],
      },
      {
        path: 'list-event/display-event/:id',
        component: DisplayEventComponent,
        canActivate: [authGuard],
      },
      {
        path: 'favourite/display-event/:id',
        component: DisplayEventComponent,
        canActivate: [authGuard],
      },
      {
        path: 'create-event',
        component: CreateEventComponent,
        canActivate: [authGuard],
      },
      {
        path: 'favourite',
        component: FavouriteEventsComponent,
      },
      { path: '**', component: PageNotFoundComponent },
    ],
  },
  { path: '**', component: PageNotFoundComponent },
  // { path: '', redirectTo: 'home', pathMatch: 'full' },
];
