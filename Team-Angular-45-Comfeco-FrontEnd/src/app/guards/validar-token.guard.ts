import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, CanLoad, Router } from '@angular/router';
import { AuthService } from '../auth/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class ValidarTokenGuard implements CanActivate, CanActivateChild {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Promise<boolean> | boolean {

    return this.userHasTokenOrPersistentLogin();

  }
  canActivateChild():Promise<boolean> | boolean{

    return this.userHasTokenOrPersistentLogin();
    
  }


  async userHasTokenOrPersistentLogin():Promise<boolean>{
    if(this.authService.isLoggedIn){
      return true;
    }

    const result = await this.authService.userHavePersistLogin;

    if(result){
      this.authService.checkSessionRecover(location.pathname);
      return true;
    }

    this.router.navigate(['/auth','login']);

    return false;

}

}

