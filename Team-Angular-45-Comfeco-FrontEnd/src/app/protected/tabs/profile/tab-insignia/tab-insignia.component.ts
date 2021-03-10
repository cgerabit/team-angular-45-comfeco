import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../auth/services/auth.service';
import { UserBadges } from '../../../../auth/interfaces/interfaces';

@Component({
  selector: 'app-tab-insignia',
  templateUrl: './tab-insignia.component.html',
  styleUrls: ['./tab-insignia.component.scss']
})
export class TabInsigniaComponent implements OnInit {

  constructor(private authService:AuthService) { }

  userBadges:UserBadges[] =[];
  ngOnInit(): void {
    this.authService.userBadges.then(b=>{
      this.userBadges= b;
    })
  }

}
