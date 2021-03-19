import { Component, OnInit } from '@angular/core';
import { Comunity } from 'src/app/protected/interfaces/interfaces';
import { HomepageService } from '../../../protected/services/homepage.service';
import { LoadingOverlayService } from '../../../shared/services/loading-overlay.service';

@Component({
  selector: 'app-comunities',
  templateUrl: './comunities.component.html',
  styleUrls: ['./comunities.component.scss']
})
export class ComunitiesComponent implements OnInit {

  constructor(private homePageService:HomepageService,
    private loadingOverLay:LoadingOverlayService) { }

  comunities:Comunity[]=[];
  ngOnInit(): void {
    this.loadingOverLay.setTimerWith(this.homePageService.getComunidades({
      Page:1,
      RecordsPerPage:150
    })).then(r =>{
      this.comunities=r;
    }).catch()
  }

  openUrl(url:string)
  {
    window.open(url);
  }

}
