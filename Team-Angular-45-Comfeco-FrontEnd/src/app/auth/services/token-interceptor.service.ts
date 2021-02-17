import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {

  constructor(private authService:AuthService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if(!req.headers.has('Authorization')){
    if(this.authService.isLoggedIn){
      const token = this.authService.token;
      req=req.clone(
        {
          setHeaders:{Authorization:`Bearer ${token}`}
        }
      )
    }
  }

    return next.handle(req);

  }
}
