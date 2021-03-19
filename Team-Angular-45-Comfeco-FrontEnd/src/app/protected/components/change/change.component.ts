import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../auth/services/auth.service';
import Swal from 'sweetalert2';
import { LoadingOverlayService } from '../../services/loading-overlay.service';


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
    private authService:AuthService,
    private loadingOverlay:LoadingOverlayService) {}

    currentPassword:string;
    changedValue:string;
  update(){

    if(!this.changedValue){
      return;
    }

    switch(this.dataModal.label){
        case "Username":
          {
            this.loadingOverlay.setTimerWith(this.authService.changeUsername({
              password:this.currentPassword,
              newUsername:this.changedValue
            })).then(resp => {

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
            }).catch(()=>{


                Swal.fire(
                  {title:"Error!",
                 text:"Contraseña invalida",
                 icon:"error"
                }
                )

            })
            .finally(()=>{
              this.activeModal.close();
              this.authService.tryRenewToken(true);
            })
            break;

          }
        case "Email":
          {
              this.loadingOverlay.setTimerWith(this.authService.changeEmail({
                password:this.currentPassword,
                newEmail:this.changedValue
              })).then(resp => {
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
              }).catch(()=>{

                  Swal.fire(
                    {title:"Error!",
                   text:"Contraseña invalida",
                   icon:"error"
                  }
                  )


              }).finally(()=>{
                this.activeModal.close();
                this.authService.tryRenewToken(true)
              })
              break;
          }
        case "Contraseña":{

          this.loadingOverlay.setTimerWith(this.authService.changePassword(
           {
            currentPassword:this.currentPassword,
            newPassword:this.changedValue
           }
          ))
          .then(()=> {
            Swal.fire(
              {title:"Exitoso!",
             text:"Has cambiado tu contraseña con exito",
             icon:"success"
            }
            )

          }).catch(()=>{


              Swal.fire(
                {title:"Error!",
               text:"La nueva contraseña no cumple con los requisitos o tu contraseña actual es invalida",
               icon:"error"
              }
              )

            }
          ).finally(()=>this.activeModal.close())
          break;
          }


    }
  }
}
