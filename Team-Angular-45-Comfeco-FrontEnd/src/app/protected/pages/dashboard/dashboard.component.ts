import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';
import * as countdown from 'countdown';

// import Swiper core and required modules
import SwiperCore, {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  Autoplay
} from 'swiper/core';
// install Swiper modules
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y, Autoplay]);


interface Time {
  days: number,
  hours: number,
  minutes: number,
  seconds: number
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  styles: [
    `
    *{
      margin: 15px;
    }`
  ]
})



export class DashboardComponent implements OnInit {

  //tiempo
  time:Time = null;
  timerId: number = null;

  //datos del servicio
  date = new Date("2022-02-23: 00:00");
  desc:string = 'Inicio del Proyecto'


  onSwiper(swiper) {
    console.log(swiper);
  }
  onSlideChange() {
    console.log('slide change');
  }

  constructor() { }

  ngOnInit(): void {

    if( new Date() <= this.date){
          this.timerId = countdown( this.date,(ts) => {
            if(ts.value < 0){
              this.time = ts;
            }else{
              this.time = {
                days:0,
                hours:0,
                minutes:0,
                seconds:0
              }
              clearInterval(this.timerId);
            }
            console.log(this.time);
            console.log(this.date);
      }, countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS);

    }else{
        this.time = {
          days:0,
          hours:0,
          minutes:0,
          seconds:0
        }
    }

  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if(this.timerId){
      clearInterval(this.timerId);
    }
  }

}
