import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AuthLayoutComponent } from '../layouts/auth-layout/auth-layout.component';
import { RecoverComponent } from './pages/recover/recover.component';
import { TermsComponent } from './pages/terms/terms.component';

const routes: Routes = [

  {
    path:'',
    component: AuthLayoutComponent,
    children:[
      { path: 'login', component: LoginComponent},
      { path: 'registro', component: RegisterComponent},
      { path: 'recover', component: RecoverComponent},
      { path: 'terms', component: TermsComponent},
      { path: '**', redirectTo: 'login'}
    ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
