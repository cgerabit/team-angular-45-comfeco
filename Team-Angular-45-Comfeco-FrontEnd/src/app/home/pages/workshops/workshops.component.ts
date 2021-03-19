import { Component, OnInit } from '@angular/core';
import { HomepageService } from '../../../protected/services/homepage.service';
import { WorkShop } from '../../../protected/interfaces/interfaces';
import { LoadingOverlayService } from '../../../shared/services/loading-overlay.service';

@Component({
  selector: 'app-workshops',
  templateUrl: './workshops.component.html',
  styleUrls: ['./workshops.component.scss']
})
export class WorkshopsComponent implements OnInit {

  constructor(private homePageService:HomepageService,
    private loadingOverlayService:LoadingOverlayService) { }

  workshops:WorkShop[] = [];
  ngOnInit(): void {
    this.loadingOverlayService
    .setTimerWith(this.homePageService.getWorshops())
    .then(r=>{
      this.workshops= r;
    })
  }

  IsGreaterThanCurrentDate(date:Date){

    if ( typeof date == "string"){
      date = new Date(date);
    }
    return date.getTime() > new Date().getTime();
  }

}
