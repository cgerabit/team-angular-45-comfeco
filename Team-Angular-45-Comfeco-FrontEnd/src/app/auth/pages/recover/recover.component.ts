import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import Swal from 'sweetalert2';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-recover',
  templateUrl: './recover.component.html',
  styleUrls: ['./recover.component.scss']
})
export class RecoverComponent implements OnInit {

  flag:boolean = false;
  recove ={
    token: null,
    user_id: null
  }

  miFormulario: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.pattern(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]],
  })

  miFormularioNuevo: FormGroup = this.fb.group(
    {
      pass: ['', [Validators.required, Validators.pattern(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$/)]],
      pass2: ['', [Validators.required, Validators.minLength(6)]],
    },
    { validators: this.checkPasswords }
  );

  constructor(
    private activateRoute: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {

  }

  ngOnInit(): void {
    this.activateRoute.queryParams.subscribe( params =>{
      this.recove.token = params.Token;
      this.recove.user_id = params.UserId;

     if(this.recove.token != undefined && this.recove.user_id != undefined){
      this.flag = true;
     }else{
      this.flag = false;
     }
    })
  }
  get emailNoValido(){
    return this.miFormulario.get('email').invalid && this.miFormulario.get('email').touched
  }
  get passwordNoValido(){
    return this.miFormularioNuevo.get('pass').invalid && this.miFormularioNuevo.get('pass').touched
  }
  get password2NoValido(){
    const password1 = this.miFormularioNuevo.get('pass').value;
    const password2 = this.miFormularioNuevo.get('pass2').value;

    return (password1 == password2) ? false: true;
  }

  checkPasswords(group: FormGroup) {
    // here we have the 'passwords' group
    const password = group.get('pass').value;
    const password2 = group.get('pass2').value;

    return password === password2 ? null : { notSame: true };
  }

  cambiar(){
    Swal.fire('Info', "Estamos actualizando su clave, un momento.", 'info');
    Swal.showLoading();
    const {pass, pass2} = this.miFormularioNuevo.value;
    this.authService.cambiarClave(this.recove.user_id, this.recove.token, pass).subscribe( resp =>{
      Swal.fire('Info', "Clave actualizada, puedes iniciar sesion.", 'info');
      if(resp.status === 200){
        Swal.fire('Info', "Clave actualizada, puedes iniciar sesion.", 'info');
      }
      if(resp.status === 400){
        Swal.fire('Error', "Ocurrio un error al actualizar la clave", 'error');
      }
    })
  }

  recover(){
    Swal.fire('Info', "Estamos enviando link a su correo", 'info');
    Swal.showLoading();

    const {email} = this.miFormulario.value;
    this.authService.recover( email )
      .subscribe(resp=>{
        Swal.fire('Info', "Hemos enviado un link a su correo.", 'info');
      }, err => {
        Swal.fire('Error', "Ocurrio un error en el momento de enviar link a su correo, intente nuevamente.", 'error');
      })

  }

}
