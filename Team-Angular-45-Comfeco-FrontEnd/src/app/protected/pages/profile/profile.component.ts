import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbNav } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  @ViewChild("nav")
  nav:NgbNav;
  ngOnInit(): void {
  }

  navigateToEvents(){
    this.nav.select("ngb-nav-3")
  }


}
