import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  var myToken = localStorage.getItem('myToken');
  console.log('Token: ', myToken);
  if (myToken) {
    const request = req.clone({
      headers: new HttpHeaders().set('Authorization', `Bearer ${myToken}`),
    });
    return next(request);
  }
  return next(req);
};
