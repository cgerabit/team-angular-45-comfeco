import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../auth/services/auth.service';
import Swal from 'sweetalert2';


interface DataModal {
  label: string;
  isPassword: boolean;
  inputData: string;
}

@Component({
  selector: 'app-change',
  templateUrl: './change.component.html',
  styleUrls: ['./change.component.scss'],
})
export class ChangeComponent {
  @Input() dataModal: DataModal;

  constructor(public activeModal: NgbActiveModal,
    private authService:AuthService) {}

    currentPassword:string;
    changedValue:string;
  update(){

    if(!this.changedValue){
      return;
    }

    switch(this.dataModal.label){
        case "Username":
          {
            this.authService.changeUsername({
              password:this.currentPassword,
              newUsername:this.changedValue
            }).subscribe(resp => {

              if(resp["needConfirm"]){
                  Swal.fire(
                    {title:"Exitoso!",
                   text:"Debes confirmar tu email para finalizar el cambio",
                   icon:"success"
                  }
                  )
                  return;
              }
              Swal.fire(
                {title:"Exitoso!",
               text:"Se ha cambiado tu usuario!",
               icon:"success"
              }

              )
            },()=> {

              Swal.fire(
                {title:"Error!",
               text:"Ha ocurrido un error actualizando el username ",
               icon:"error"
              }
              )
              this.activeModal.close();
            })
            this.activeModal.close();
          }
        case "Email":
          {
              this.authService.changeEmail({
                password:this.currentPassword,
                newEmail:this.changedValue
              }).subscribe(resp => {
                if(resp["needConfirm"]){
                  Swal.fire(
                    {title:"Exitoso!",
                   text:"Debes confirmar tu email para finalizar el cambio",
                   icon:"success"
                  }
                  )
                  return;
              }
              Swal.fire(
                {title:"Exitoso!",
               text:"Se ha cambiado tu Email!",
               icon:"success"
              }
              )
              this.activeModal.close();
              },()=> {
                Swal.fire(
                  {title:"Error!",
                 text:"ha ocurrido un error actualizando el username",
                 icon:"error"
                }
                )
                this.activeModal.close();
              })
          }
        case "Contraseña":{

          this.authService.changePassword(
           {
            currentPassword:this.currentPassword,
            newPassword:this.changedValue
           }
          )
          .subscribe(()=> {
            Swal.fire(
              {title:"Exitoso!",
             text:"Has cambiado tu contraseña con exito",
             icon:"success"
            }
            )
            this.activeModal.close();
          },()=>{

            Swal.fire(
              {title:"Error!",
             text:"La nueva contraseña no cumple con los requisitos o tu contraseña actual es invalida",
             icon:"error"
            }
            )
            this.activeModal.close();
          })
          }


    }
  }
}
