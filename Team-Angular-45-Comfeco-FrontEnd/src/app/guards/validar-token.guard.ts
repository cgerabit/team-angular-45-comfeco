import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/services/auth.service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ValidarTokenGuard implements CanActivate, CanLoad {


  constructor( private authService:AuthService,
                private router: Router){

  }

  canActivate(): Observable<boolean>  | boolean {
    //console.log('canActivate');

    return this.authService.validarToken()
      .pipe(
        tap( valid => {
          if(!valid){
            this.router.navigateByUrl('/auth/login')
          }
        })
      );
  }
  canLoad(): Observable<boolean> |boolean {
    //console.log('canLoad');
    return this.authService.validarToken()
      .pipe(
        tap( valid => {
          if(!valid){
            this.router.navigateByUrl('/auth/login')
          }
        })
      );
  }
}
