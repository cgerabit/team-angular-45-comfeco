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
import { Event, Technologies, Comunity, Sponsor, ContentCreator } from '../../interfaces/interfaces';
import { HomepageService } from '../../services/homepage.service';
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

  areas:  any = [];
  comunidades :Comunity[]=[];
  sponsors:Sponsor[] = [];

  contentCreators:ContentCreator[];
  //tiempo
  time:Time = null;
  timerId: number = null;

  //datos del servicio
  date:Date;
  desc:string;
  finalizado:boolean = false;



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
    slidesPerView:2,

    breakpoints: {
      360:{
        slidesPerView:3
      },
      768: {
        slidesPerView: 5,
      },
      1024: {
        slidesPerView: 6
      },
      1368:{
        slidesPerView: 7
      }
    }
  };

  public onIndexChange(index: number): void {
    //console.log('Swiper index: ', index);
  }

  public onSwiperEvent(event: string): void {
    //console.log('Swiper event: ', event);
  }



  constructor(private hs:HomepageService) {

   }


  ngOnInit(): void {

    this.loadData();

  }

  loadData(){
    this.hs.getTecnologias().subscribe((resp:Technologies[])=>{
      this.areas = resp;
  });

  //Comunidades
  this.hs.getComunidades({
    Page:1,
    RecordsPerPage:4
  })
  .subscribe(resp => {
    this.comunidades=resp;
  },err => {
    console.log("Ha ocurrido un error",err);
  })

  this.hs.getContentCreators({
    Page:1,
    RecordsPerPage:100
  })
  .subscribe(resp => {

    this.contentCreators = resp;

  },err=> console.log(err))
  //Sponsors
  this.hs.getSponsors({
    Page:1,
    RecordsPerPage:100
  })
  .subscribe(resp => {
    this.sponsors = resp;
  },
  err => console.log(err))




  this.hs.eventInfo().subscribe((resp:Event[])=>{
    this.date =  new Date(resp[0].date);
    //fecha pruebas
    this.date = new Date("2021-02-25 20:58");
    this.desc = resp[0].name;

    //funcionalidad del contador
    this.contador(this.date);
  });
  }

  //funcionalidad del contador
  contador(date:Date):void{
     //Comprobar si la fecha del evento ya fue vencida
     if( new Date() <= date){
      this.timerId = countdown( date,(ts) => {
            //comprobar que la fecha actual sea menor a la fecha del evento
            if(ts.value < 0){
              this.time = ts;
            }else{
              this.finalizado=true;
              clearInterval(this.timerId);
            }
            //console.log(this.time);
            //console.log(this.date);
      }, countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS);

    }else{
        this.finalizado=true;
    }
  }

  ngOnDestroy(): void {
    //Detener el contador al destruir el elemento
    if(this.timerId){
      clearInterval(this.timerId);
    }
  }

}
