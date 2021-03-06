import { Component, OnInit } from '@angular/core';
import { UserProfile } from 'src/app/auth/interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { AuthService } from '../../../../auth/services/auth.service';
import { Technologies, Area, applicationUserSocalNetworks } from '../../../interfaces/interfaces';

@Component({
  selector: 'app-tab-profile',
  templateUrl: './tab-profile.component.html',
  styleUrls: ['./tab-profile.component.scss']
})
export class TabProfileComponent implements OnInit {

  public slidesSponsor = [
    'https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/Angular_full_color_logo.svg/1200px-Angular_full_color_logo.svg.png',
    'https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/1200px-React-icon.svg.png',
    'https://www.manejandodatos.es/wp-content/uploads/2018/02/vueJS.png',
    'https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Svelte_Logo.svg/1200px-Svelte_Logo.svg.png',
  ];


  constructor(private authService:AuthService,
    private homeService:HomepageService) { }

  profile:UserProfile;
  userSpecialty:Area;

  userSocialNetworks:applicationUserSocalNetworks[];

  get usuario(){
    const { userName } = this.authService.userInfo;
    return {
      userName: userName,
      avatar:
        `https://avatars.dicebear.com/api/bottts/${userName}.svg`,
    };
  }

  ngOnInit(): void {

    this.authService.userProfile.then(r => this.profile = r );
    this.authService.userSocialNetworks.then(r=> this.userSocialNetworks = r );
    this.authService.userSpecialty.then(r => this.userSpecialty = r );

  }

}
