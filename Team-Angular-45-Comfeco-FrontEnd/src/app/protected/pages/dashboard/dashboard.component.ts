import { Component, OnInit } from '@angular/core';
import * as countdown from 'countdown';

import SwiperCore, {
  Navigation,
  Pagination,
  Scrollbar,
  A11y,
  Autoplay,
  SwiperOptions,
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
})




export class DashboardComponent implements OnInit {

  //tiempo
  time:Time = null;
  timerId: number = null;

  //datos del servicio
  date = new Date("2022-02-23: 00:00");
  desc:string = 'Inicio del Proyecto'

  public slides = [
    'https://www.comfeco.com/images/leaders/leader-bezael_perez.webp',
    'https://www.comfeco.com/images/leaders/leader-manuel_ojeda.webp',
    'https://www.comfeco.com/images/leaders/leader-ignacio_anaya.webp',
    'https://www.comfeco.com/images/leaders/leader-marcos_rivas.webp',
    'https://www.comfeco.com/images/leaders/leader-diego_montoya.webp',
    'https://www.comfeco.com/images/leaders/leader-Martin_Bavio.webp',
    'https://www.comfeco.com/images/leaders/leader-fernando_de_la_rosa.webp',
    'https://www.comfeco.com/images/leaders/leader-manuel_mu%C3%B1os.webp',
  ];
  public slidesSponsor = [
    'https://www.comfeco.com/images/sponsors/sponsor-huawei.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-fernando_herrera.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-leonidas_esteban.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-egghead.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-codigofacilito.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-latamdev.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-codelytv.webp',
    'https://www.comfeco.com/images/sponsors/sponsor-tekkitv.webp',
  ];


  public config: SwiperOptions = {
    a11y: { enabled: true },
    direction: 'horizontal',
    keyboard: true,
    mousewheel: true,
    scrollbar: false,
    loop: true,
    autoplay: {
      disableOnInteraction: false,
    },
    centeredSlides: false,
    centeredSlidesBounds: true,
    spaceBetween: 10,
    breakpoints: {
      640: {
        slidesPerView: 2,
      },
      1024: {
        slidesPerView: 3,
      },
    },
  };


  public config2: SwiperOptions = {
    a11y: { enabled: true },
    direction: 'horizontal',
    keyboard: true,
    mousewheel: true,
    scrollbar: false,
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
    pagination:{
      clickable: true,
    },
    /*pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable:true,
      dynamicBullets: true,
    },*/
    loop:true,
    autoplay:false,
    centeredSlides:false,
    centeredSlidesBounds:true,
    slidesPerView:3,

    breakpoints: {
      640: {
        slidesPerView: 3,
      },
      768: {
        slidesPerView: 5,
      },
      1024: {
        slidesPerView: 7
      },
    }
  };

  public onIndexChange(index: number): void {
    //console.log('Swiper index: ', index);
  }

  public onSwiperEvent(event: string): void {
    //console.log('Swiper event: ', event);
  }


  constructor() { }

  ngOnInit(): void {
    //Comprobar si la fecha del evento ya fue vencida
    if( new Date() <= this.date){
          this.timerId = countdown( this.date,(ts) => {
            //comprobar que la fecha actual sea menor a la fecha del evento
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
            //console.log(this.time);
            //console.log(this.date);
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
    //Detener el contador al destruir el elemento
    if(this.timerId){
      clearInterval(this.timerId);
    }
  }

}
