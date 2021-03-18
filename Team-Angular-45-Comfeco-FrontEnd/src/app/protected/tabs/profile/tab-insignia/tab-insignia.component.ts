import { Component, OnInit } from '@angular/core';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { UserBadges } from '../../../../auth/interfaces/interfaces';
import { LoadingOverlayService } from '../../../services/loading-overlay.service';

@Component({
  selector: 'app-tab-insignia',
  templateUrl: './tab-insignia.component.html',
  styleUrls: ['./tab-insignia.component.scss'],
})
export class TabInsigniaComponent implements OnInit {
  constructor(
    private homepageServices: HomepageService,
    private loadingOverlay: LoadingOverlayService
  ) {}

  badges: any[] = [];
  ngOnInit(): void {
    this.loadingOverlay
      .setTimerWith(this.homepageServices.getBadges())
      .then((resp) => {
        this.badges = resp;
      })
      .catch(() => {});
  }
}
