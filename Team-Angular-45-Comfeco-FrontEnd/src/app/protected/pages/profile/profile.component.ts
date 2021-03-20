import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbNav, NgbNavItem } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private activatedRoute:ActivatedRoute){

  }
  @ViewChild("nav")
  nav:NgbNav;
  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params =>
      {
        if(params.msg){
          Swal.fire({
            title:'Importante',
            text:params.msg,
            icon:'info'
          });
        }
      })
  }

  navigateToEvents(){
    if(this.nav){
      var tabs = this.nav.items.toArray();
      var eventsTab = tabs[tabs.length-1] as NgbNavItem;
      if(!eventsTab){return;}
      this.nav.select(eventsTab.domId)
    }
  }


}
