import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SwiperModule } from 'swiper/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeRoutingModule } from './home-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SharedModule as SharedModule } from '../shared/shared.module';
import { ComunitiesComponent } from './pages/comunities/comunities.component';
import { WorkshopsComponent } from './pages/workshops/workshops.component';
import { ContentCreatorsComponent } from './pages/content-creators/content-creators.component';
import { HomeLayoutComponent } from '../layouts/home-layout/home-layout.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [DashboardComponent,HomeLayoutComponent, ComunitiesComponent, WorkshopsComponent, ContentCreatorsComponent],
  imports: [
    CommonModule,
    SwiperModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    HomeRoutingModule,
    RouterModule
  ]
})
export class HomeModule { }
