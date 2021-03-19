import { Component, OnInit } from '@angular/core';
import { HomepageService } from '../../../protected/services/homepage.service';
import { LoadingOverlayService } from '../../../shared/services/loading-overlay.service';
import { ContentCreator } from '../../../protected/interfaces/interfaces';

@Component({
  selector: 'app-content-creators',
  templateUrl: './content-creators.component.html',
  styleUrls: ['./content-creators.component.scss']
})
export class ContentCreatorsComponent implements OnInit {

  constructor(private homeService:HomepageService,
    private loadingOverlay:LoadingOverlayService) { }

    contentCreators:ContentCreator[] = [];
  ngOnInit(): void {

    this.loadingOverlay.setTimerWith(this.homeService.getContentCreators({
      Page:1,
      RecordsPerPage:150
    }))
    .then(r=>{
      this.contentCreators = r;
    })
    .catch();
  }

}
