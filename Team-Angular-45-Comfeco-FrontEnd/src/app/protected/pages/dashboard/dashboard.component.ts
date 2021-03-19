import { ValueConverter } from '@angular/compiler/src/render3/view/template';
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
import { Event, Technologies, Comunity, Sponsor, ContentCreator, WorkShop, Area } from '../../interfaces/interfaces';
import { HomepageService } from '../../services/homepage.service';
import { LoadingOverlayService } from '../../services/loading-overlay.service';
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

  areas:  Area[] = [];
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

  workshopFilter=
  {
    id:-1
  }


  IsGreaterThanCurrentDate(date:Date){

    if ( typeof date == "string"){
      date = new Date(date);
    }
    return date.getTime() > new Date().getTime();
  }

  public config: SwiperOptions = {
    a11y: { enabled: true },
    direction: 'horizontal',
    keyboard: true,
    mousewheel: true,
    scrollbar: false,
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

workshops:WorkShop[] = [];

  constructor(private hs:HomepageService,
    private loadingOverlay:LoadingOverlayService) {

   }


  ngOnInit(): void {

    this.loadData();

  }

  loadData(){

    this.loadingOverlay.setTimerWith(this.hs.getAreas({Page:1,RecordsPerPage:150})).then((resp:Area[])=>{
      this.areas = resp;
  }).catch();

  //Comunidades
  this.loadingOverlay.setTimerWith(this.hs.getComunidades({
    Page:1,
    RecordsPerPage:4
  }))
  .then(resp => {
    this.comunidades=resp;
  }).catch();

  this.loadingOverlay.setTimerWith(this.hs.getContentCreators({
    Page:1,
    RecordsPerPage:100
  }))
  .then(resp => {

    this.contentCreators = resp;

  }).catch()
  //Sponsors
  this.loadingOverlay.setTimerWith(this.hs.getSponsors({
    Page:1,
    RecordsPerPage:100
  }))
  .then(resp => {
    this.sponsors = resp;
  }).catch()


this.loadWorkShops();


  this.hs.eventInfo().subscribe((resp:Event[])=>{
    this.date =  new Date(resp[0].date);
    //fecha pruebas
    this.date = new Date("2021-02-25 20:58");
    this.desc = resp[0].name;

    //funcionalidad del contador
    this.contador(this.date);
  });
  }



  loadWorkShops(selectedArea?:number ){


    if(selectedArea ==-1){
        selectedArea=null;
    }


    this.loadingOverlay.setTimerWith(this.hs.getWorshops(selectedArea))
    .then( w => this.workshops = w).catch();
  }

  loadWorkShopsWithEvent(selectedArea:any){

    if(selectedArea.target && selectedArea.target.value ){
      let value = selectedArea.target.value;


      this.loadWorkShops(value);
    }

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
