import { Component, OnInit } from '@angular/core';
import { HomepageService } from '../../../services/homepage.service';
import { Group } from '../../../interfaces/interfaces';

@Component({
  selector: 'app-tab-group',
  templateUrl: './tab-group.component.html',
  styleUrls: ['./tab-group.component.scss']
})
export class TabGroupComponent implements OnInit {

  constructor( private homepageService:HomepageService) { }

  groups:Group[]=[];
  ngOnInit(): void {

    this.homepageService.getGroups({}).subscribe(resp => {
      this.groups =resp;
    }, err => console.log(err))
  }

}
