import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { MainComponent } from './pages/main/main.component';
import { RecoverComponent } from './pages/recover/recover.component';
import { TermsComponent } from './pages/terms/terms.component';
import { ExternalSigninCallbackComponent } from './external-signin-callback/external-signin-callback.component';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    MainComponent,
    RecoverComponent,
    TermsComponent,
    ExternalSigninCallbackComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule
  ]
})
export class AuthModule { }
