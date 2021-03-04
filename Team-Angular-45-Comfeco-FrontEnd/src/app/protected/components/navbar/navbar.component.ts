import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(  private router: Router,
          private authService: AuthService) { }

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