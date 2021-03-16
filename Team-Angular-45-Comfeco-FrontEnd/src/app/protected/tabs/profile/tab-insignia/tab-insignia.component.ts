import { Component, OnInit } from '@angular/core';
import { UserBadges } from '../../../../auth/interfaces/interfaces';
import { UserService } from '../../../../auth/services/user.service';

@Component({
  selector: 'app-tab-insignia',
  templateUrl: './tab-insignia.component.html',
  styleUrls: ['./tab-insignia.component.scss']
})
export class TabInsigniaComponent implements OnInit {

  constructor(private userService:UserService) { }

  userBadges:UserBadges[] =[];
  ngOnInit(): void {
    this.userService.userBadges.then(b=>{
      this.userBadges= b;
    })
  }

}
