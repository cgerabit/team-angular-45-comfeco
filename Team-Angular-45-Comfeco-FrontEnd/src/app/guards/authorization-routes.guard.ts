import { Injectable } from '@angular/core';
import { CanActivate, Router, CanLoad, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationRoutesGuard implements CanActivate ,CanActivateChild {
  constructor(private authService:AuthService,
    private router:Router){

  }

  canActivate(): Observable<boolean> | boolean {

      const result = !this.authService.isLoggedIn;

      if(!result){

        this.router.navigate(['/protected'])
      }

      return result;
    }



    canActivateChild():Observable<boolean> | boolean{
      const result = !this.authService.isLoggedIn;

      if(!result){

        this.router.navigate(['/protected'])
      }

      return result;
    }

}
