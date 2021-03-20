import { Component, OnInit } from '@angular/core';
import { HomepageService } from '../../../services/homepage.service';
import { Group, GroupFilter, Technologies } from '../../../interfaces/interfaces';
import { UserGroup } from '../../../../auth/interfaces/interfaces';
import Swal from 'sweetalert2';
import { UserService } from '../../../../auth/services/user.service';
import { LoadingOverlayService } from '../../../../shared/services/loading-overlay.service';

@Component({
  selector: 'app-tab-group',
  templateUrl: './tab-group.component.html',
  styleUrls: ['./tab-group.component.scss']
})
export class TabGroupComponent implements OnInit {

  constructor( private homepageService:HomepageService,
    private userService:UserService,
    private loadingOverlay:LoadingOverlayService) { }

  groupFilter:GroupFilter = {
    TechnologyId : -1
  }
  groupTimeout:NodeJS.Timeout;
  userGroup:UserGroup;
  technologies:Technologies[] = [];

  avatar(name: string){
    return `https://avatars.dicebear.com/api/bottts/${name}.svg`
  }

  set nameFilter(value:string){

    this.groupFilter.Name = value;

    if(this.groupTimeout){
      clearTimeout(this.groupTimeout);
    }

    this.groupTimeout = setTimeout(() => {
      this.loadGroups()
    }, (320));

  }

  get nameFilter():string{

    return this.groupFilter.Name;
  }

  set technologiesFilter(value:number){


    this.groupFilter.TechnologyId = value;
    if(this.groupTimeout){
      clearTimeout(this.groupTimeout);
    }

    this.groupTimeout = setTimeout(() => {
      this.loadGroups()
    }, (320));

  }

  get technologiesFilter():number{

    return this.groupFilter.TechnologyId;
  }

  groups:Group[]=[];
  ngOnInit(): void {

    this.loadGroups();

    this.loadMyGroup();
    this.homepageService.getTecnologias()
    .subscribe(resp => {
      this.technologies = resp;
    })
  }



  loadGroups(){

    this.loadingOverlay.setTimerWith(this.homepageService.getGroups(this.groupFilter)).then(resp => {
      this.groups =resp;
    }).catch(()=>{})
  }

  loadMyGroup(){

    this.loadingOverlay.setTimerWith(this.userService
    .getUserGroup(false))
    .then(resp => {
      this.userGroup = resp;
    })

  }
  leaveMeFromGroup(){
    if(this.loadingOverlay.loadingOverlayVisible){
      return;
    }

    Swal.fire({
      title: '¿Estas seguro que quieres salir del grupo?',
      text: "No podras revertir esta accion",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, quiero salir'
    }).then(result => {
        if(result.isConfirmed){

          this.loadingOverlay.setTimerWith(this.userService.leaveUserFromGroup())
          .then(result => {
            if(result){
                this.loadMyGroup();
                Swal.fire({
                  title:"Exito",
                  text:"Has salido del grupo",
                  icon:"success"
                })
                return;
            }
            Swal.fire({
              title:"Error",
              text:"Ha ocurrido un error mientras salias del grupo",
              icon:"error"
            })

          })

        }
    })
  }

  joinInGroup(groupId:number){
    if(this.loadingOverlay.loadingOverlayVisible){
      return;
    }

    this.loadingOverlay.setTimerWith(this.userService.addUserInAGroup(groupId))
    .then(resp=> {
      if(resp.success){
        this.loadMyGroup();
        Swal.fire({
          title:"Exito",
          text:"Te has unido al grupo",
          icon:"success"
        })

      }
      else if(resp.alreadyInAgroup){
        if(resp.alreadyInThisGroup){
          Swal.fire({
            title:"Error",
            text:"Ya estas en ese grupo",
            icon:"error"
          })
        }
        else{
          Swal.fire({
            title:"Error",
            text:"Ya estas en un grupo",
            icon:"error"
          })
        }
      }
      else{
        Swal.fire({
          title:"Exito",
          text:"Ha ocurrido un error mientras te unias al grupo",
          icon:"success"
        })
      }
    })
  }

  isMyGroupId(groupId:number):boolean{

    if(!this.userGroup){
      return false;
    }

     return groupId == this.userGroup.groupId;


  }

}
