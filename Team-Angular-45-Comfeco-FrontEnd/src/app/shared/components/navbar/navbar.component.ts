import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';
import { UserProfile } from '../../../auth/interfaces/interfaces';
import { UserService } from '../../../auth/services/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(  private router: Router,
          public authService: AuthService,
          private userService:UserService) { }



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
  Profile:UserProfile
  ngOnInit(): void {

    this.userService.userProfile.then(resp => {
      if(resp){
        this.Profile = resp;
      }
    })

    this.userService.profileChanged.subscribe( p => {
      this.Profile = p;
    })
  }

  logout(){
    this.router.navigateByUrl('/auth/login');
    this.authService.logout();
  }

  //abrir el menu al presionar sobre el boton
  togleResponsiveNavbar(): void {
    this.sideNavIsOpen = !this.sideNavIsOpen;
  }

}
