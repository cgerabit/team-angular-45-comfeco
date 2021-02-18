import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Router } from '@angular/router';
import { AuthService } from '../auth/services/auth.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ValidarTokenGuard implements CanActivate, CanLoad {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Promise<boolean> | boolean {

    return this.userHasTokenOrPersistentLogin();

  }
  canLoad(): Promise<boolean> | boolean {

    return this.userHasTokenOrPersistentLogin();
  }

  userHasTokenOrPersistentLogin():Promise<boolean>{
    return new Promise((resolve)=>{


      if(this.authService.isLoggedIn){
        resolve(true);
        return;
       }

      this.authService.userHavePersistLogin.then(resp => {
        if(resp){
          this.authService.initPersistentLogin(environment.persistLoginEndpoint,location.pathname)
        }
        else{
          resolve(false);
          this.router.navigate(['/auth/login'])
        }
      }).catch(()=>{
        resolve(false);
        this.router.navigate(['/auth/login'])
      })

    })
  }


}

