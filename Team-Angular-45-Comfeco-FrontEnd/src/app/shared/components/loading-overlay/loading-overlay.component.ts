import { Component, OnInit } from '@angular/core';
import { LoadingOverlayService } from '../../services/loading-overlay.service';


@Component({
  selector: 'app-loading-overlay',
  templateUrl: './loading-overlay.component.html',
  styleUrls: ['./loading-overlay.component.scss']
})
export class LoadingOverlayComponent implements OnInit {

  constructor(private loadingOverlayService:LoadingOverlayService) { }

  ngOnInit(): void {
    this.loadingOverlayService.overlayChanged.subscribe(v =>{
      this.overlayVisible = v ;
    } )
  }

  overlayVisible:boolean=false;

}

