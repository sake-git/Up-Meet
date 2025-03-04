import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  console.log('Auth guard called');
  var router = inject(Router);

  var token = localStorage.getItem('myToken');
  if (token && token != '') {
    return true;
  }

  router.navigateByUrl('/login');

  return false;
};
