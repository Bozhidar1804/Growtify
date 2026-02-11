import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../services/account-service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  const user = accountService.currentUser();

  if (req.url.includes('api/members')) {
      console.log('Interceptor running for:', req.url);
      console.log('Current User:', user);
      console.log('Token to send:', user?.token);
  }

  if (user?.token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${user.token}`
      }
    });
  }
  return next(req);
};
