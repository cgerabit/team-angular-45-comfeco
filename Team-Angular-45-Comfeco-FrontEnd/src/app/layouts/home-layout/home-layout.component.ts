import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';

import { Technologies } from '../../protected/interfaces/interfaces';
import { HomepageService } from '../../protected/services/homepage.service';


@Component({
  selector: 'app-home-layout',
  templateUrl: './home-layout.component.html',
  styleUrls: ['./home-layout.component.scss']
})
export class HomeLayoutComponent implements OnInit {

  areas:  any = [];

  constructor(
    private router: Router,
    private authService: AuthService,
    private hs: HomepageService
    ) { }
  //variable para abrir el menu
  sideNavIsOpen: boolean = false;
  //obtener datos del usuario
  get usuario(){
    const { userName } = this.authService.userInfo;
    return {
      userName: userName,
      avatar:
        `https://avatars.dicebear.com/api/bottts/${userName}.svg`,
    };
  }
  ngOnInit(): void {
    this.hs.getTecnologias().subscribe((resp:Technologies[])=>{
        this.areas = resp;
    });
  }

  logout(){
    this.router.navigateByUrl('/auth/login');
    this.authService.logout();
  }

  //abrir el menu al presionar sobre el boton
  togleResponsiveNavbar(): void {
    this.sideNavIsOpen = !this.sideNavIsOpen;
    console.log(this.sideNavIsOpen)
  }

}
