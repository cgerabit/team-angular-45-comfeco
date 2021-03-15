import { Component, OnInit } from '@angular/core';
import { ActiveEvent } from 'src/app/protected/interfaces/interfaces';
import { HomepageService } from '../../../services/homepage.service';
import { AuthService } from '../../../../auth/services/auth.service';
import Swal from 'sweetalert2';
import { UserEventInscriptionDTO } from '../../../../auth/interfaces/interfaces';
@Component({
  selector: 'app-tab-event',
  templateUrl: './tab-event.component.html',
  styleUrls: ['./tab-event.component.scss']
})
export class TabEventComponent implements OnInit {

  constructor( private homeService:HomepageService,
    private authService:AuthService) { }

  events:ActiveEvent[] = [];

  userEvents:UserEventInscriptionDTO[] = [];

  ngOnInit(): void {

    this.homeService.getEvents().subscribe(e=> {

      this.events = e;
    })

    this.authService.userEvents.then(r=> {
      this.userEvents =r;
    });

    this.authService.userEventsChanged.subscribe(r=>{
      this.userEvents =r;
    })
  }



  addUserToEvent(event:ActiveEvent){

    const userId = this.authService.userInfo.userId;

    if(!userId){
      return;
    }
    Swal.fire({
      title: '¿Estas seguro?',
      text: `¿Estas seguro que quieres unirte al evento ${event.name}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#F2A007',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si quiero unirme'
    }).then(r=> {

        if(r.isConfirmed){
          this.authService.addUserToEvent(event.id,userId).subscribe(()=> {
            Swal.fire({
              title:"Exito!",
              text:"Te has inscrito exitosamente en el evento",
              icon:"success"
            })
        }, ()=>{
          Swal.fire({
            title:"Error!",
            text:"Este evento esta cerrado o ya estas inscrito",
            icon:"error"
          })
        })
        }

    })
  }

  userIsInEvent(eventId:number){
    return this.userEvents.some(e=> e.eventId == eventId);
  }
  userLeaveFromEvent(eventId:number){
    return this.userEvents.some(e=> e.eventId == eventId && !e.isActive)
  }
  leaveFromEvent(event:ActiveEvent){
    Swal.fire({
      title: '¿Estas seguro?',
      text: `¿Estas seguro que quieres salir del evento ${event.name}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#F2A007',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si quiero salir'
    }).then(r =>{
      if(r.isConfirmed){
        this.authService.
        removeUserFromEvent(event.id,this.authService.userInfo.userId).subscribe(()=>{
          Swal.fire({
            title:"Exito!",
            text:"Has abandonado correctamente el evento",
            icon:"success"
          })
        },()=>{
          Swal.fire({
            title:"Error!",
            text:"Ha ocurrido un error inesperado",
            icon:"error"
          })
        });
      }
    })
  }
}
