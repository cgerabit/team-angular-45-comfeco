import { NgModule } from '@angular/core';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { Routes, RouterModule } from '@angular/router';
import { ComunitiesComponent } from './pages/comunities/comunities.component';
import { WorkshopsComponent } from './pages/workshops/workshops.component';
import { ContentCreatorsComponent } from './pages/content-creators/content-creators.component';
import { HomeLayoutComponent } from '../layouts/home-layout/home-layout.component';

const routes: Routes = [
  {
    path: '',
    component:HomeLayoutComponent,
    children:[
      {path:'dashboard', component: DashboardComponent},
      {path:'comunities',component:ComunitiesComponent},
      {path:'workshops',component:WorkshopsComponent},
      {path:'contentcreators',component:ContentCreatorsComponent},
      {path:'**', redirectTo:'dashboard'},

    ]
  }
];


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class HomeRoutingModule { }
