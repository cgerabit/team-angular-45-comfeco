import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbNav } from '@ng-bootstrap/ng-bootstrap';
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
    this.nav.select("ngb-nav-3")
  }


}
