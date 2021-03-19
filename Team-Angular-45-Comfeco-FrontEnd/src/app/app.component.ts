import { AfterViewChecked, AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { textChangeRangeIsUnchanged } from 'typescript';
import { AuthService } from './auth/services/auth.service';
import { environment } from '../environments/environment';
import { RouterEvent, Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { LoadingOverlayService } from './shared/services/loading-overlay.service';

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
constructor(private authService:AuthService,
  private router:Router,
  private loadingOverlayService:LoadingOverlayService) {
  this.router.events.subscribe((event: RouterEvent) => {
    this.navigationInterceptor(event);
  });
}

  ngOnDestroy(): void {
   if(this.renovationTokenInterval){
     clearInterval(this.renovationTokenInterval);
   }
  }

  ngAfterViewInit():void{

    this.authService.checkSessionRecover(location.pathname);
    this.renovationTokenInterval = setInterval(()=>{

      this.authService.tryRenewToken().then();

    },300000)
  }

  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.loadingOverlayService.loadingOverlayVisible = true;
    }
    if (event instanceof NavigationEnd) {
      this.loadingOverlayService.loadingOverlayVisible = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.loadingOverlayService.loadingOverlayVisible = false;
    }
    if (event instanceof NavigationError) {
      this.loadingOverlayService.loadingOverlayVisible = false;
    }
  }
  title = 'authApp';
}
