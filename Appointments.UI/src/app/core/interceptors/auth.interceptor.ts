import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const userId = this.authService.getUserId();

    if (userId) {
      const cloned = req.clone({
        setHeaders: {
          'X-User-Id': userId.toString()
        }
      });
      return next.handle(cloned);
    }

    return next.handle(req);
  }
}
