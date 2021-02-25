import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProtectedRoutingModule } from './protected-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

import { SwiperModule } from 'swiper/angular';

//import { SwiperModule } from 'ngx-swiper-wrapper';
//mport { SWIPER_CONFIG } from 'ngx-swiper-wrapper';
//import { SwiperConfigInterface } from 'ngx-swiper-wrapper';

/*const DEFAULT_SWIPER_CONFIG: SwiperConfigInterface = {
  direction: 'horizontal',
  slidesPerView: 'auto'
};*/


@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    ProtectedRoutingModule,
    SwiperModule
  ],
  /*providers: [
    {
      provide: SWIPER_CONFIG,
      useValue: DEFAULT_SWIPER_CONFIG
    }
  ]*/
})
export class ProtectedModule { }
