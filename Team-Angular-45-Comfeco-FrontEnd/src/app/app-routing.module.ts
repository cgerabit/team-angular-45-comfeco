import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ValidarTokenGuard } from './guards/validar-token.guard';
import { AuthorizationRoutesGuard } from './guards/authorization-routes.guard';


const routes: Routes = [
  {
    path:'auth',
    loadChildren: () => import('./auth/auth.module').then( m => m.AuthModule),
    canActivate:[AuthorizationRoutesGuard],
    canActivateChild:[AuthorizationRoutesGuard]
  },
  {
    path:'protected',
    loadChildren: () => import('./protected/protected.module').then( m => m.ProtectedModule),
    canActivate: [ ValidarTokenGuard ],
    canActivateChild: [ValidarTokenGuard]
  },
  {
    path:'home',
    loadChildren: () => import('./Home/Home.module').then( m => m.HomeModule)
  },
  {
    path: '**',
    redirectTo:'auth'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
