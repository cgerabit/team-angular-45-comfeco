import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProtectedRoutingModule } from './protected-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

import { SwiperModule } from 'swiper/angular';
import { ProfileComponent } from './pages/profile/profile.component';
import { NavbarComponent } from './components/navbar/navbar.component';

//import { SwiperModule } from 'ngx-swiper-wrapper';
//mport { SWIPER_CONFIG } from 'ngx-swiper-wrapper';
//import { SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TabProfileComponent } from './tabs/profile/tab-profile/tab-profile.component';
import { TabInsigniaComponent } from './tabs/profile/tab-insignia/tab-insignia.component';
import { TabGroupComponent } from './tabs/profile/tab-group/tab-group.component';
import { TabEventComponent } from './tabs/profile/tab-event/tab-event.component';

/*const DEFAULT_SWIPER_CONFIG: SwiperConfigInterface = {
  direction: 'horizontal',
  slidesPerView: 'auto'
};*/


@NgModule({
  declarations: [DashboardComponent, ProfileComponent, NavbarComponent, TabProfileComponent, TabInsigniaComponent, TabGroupComponent, TabEventComponent],
  imports: [
    CommonModule,
    ProtectedRoutingModule,
    SwiperModule,
    NgbModule
  ],
  /*providers: [
    {
      provide: SWIPER_CONFIG,
      useValue: DEFAULT_SWIPER_CONFIG
    }
  ]*/
})
export class ProtectedModule { }
