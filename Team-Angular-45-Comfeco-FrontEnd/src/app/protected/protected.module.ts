import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProtectedRoutingModule } from './protected-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

import { SwiperModule } from 'swiper/angular';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    ProtectedRoutingModule,
    SwiperModule
  ]
})
export class ProtectedModule { }
