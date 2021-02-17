import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { textChangeRangeIsUnchanged } from 'typescript';
import { AuthService } from './auth/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy, AfterViewInit  {


  private renovationTokenInterval;
/**
 *
 */
constructor(private authService:AuthService) {

}
  ngAfterViewInit(): void {

    this.renovationTokenInterval = setInterval(()=>{

      this.authService.tryRenewToken().then(result => console.log(result));

    },300000)
  }
  ngOnDestroy(): void {
   if(this.renovationTokenInterval){
     clearInterval(this.renovationTokenInterval);
   }
  }
  title = 'authApp';
}
