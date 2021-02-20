import { AfterViewChecked, AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { textChangeRangeIsUnchanged } from 'typescript';
import { AuthService } from './auth/services/auth.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy,  AfterViewInit  {


  private renovationTokenInterval;
/**
 *
 */
constructor(private authService:AuthService) {

}

  ngOnDestroy(): void {
   if(this.renovationTokenInterval){
     clearInterval(this.renovationTokenInterval);
   }
  }

  ngAfterViewInit():void{

    this.authService.checkSessionRecover(location.pathname);
    this.renovationTokenInterval = setInterval(()=>{

      this.authService.tryRenewToken().then(result => console.log(result));

    },300000)
  }

  title = 'authApp';
}
