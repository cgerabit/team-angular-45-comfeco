import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import Swal from 'sweetalert2';
import { TokenResponse } from '../../interfaces/interfaces';
import { LocalService } from '../../services/local.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  isLoading: boolean = false;
  miFormulario: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    remember: [false],
    saveSession:[false]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private localService: LocalService,
    private activatedRoute:ActivatedRoute
  ) {}

  ngOnInit(): void {
    //remember password
    let userdata = this.localService.getJsonValue('user');
    if( userdata){
      this.miFormulario.reset({remember: true, email: userdata.email ,password: userdata.password })
    }
    this.activatedRoute.queryParams.subscribe(params => {
      if(params.msg){
        Swal.fire(params.msg, "" ,"info")
      }

    });
  }



  //validaciones
  get correoNoValido(){
    return this.miFormulario.get('email').invalid && this.miFormulario.get('email').touched
  }
  get passwordNoValido(){
    return this.miFormulario.get('password').invalid && this.miFormulario.get('password').touched
  }

  change(): void {
    this.isLoading = !this.isLoading;
  }

  login() {
    //console.log(this.miFormulario.value);
    //console.log(this.miFormulario.valid);

    //this.authService.validarToken().subscribe(resp=> console.log(resp));
    if( this.miFormulario.value.remember){
      let user = {
        email: this.miFormulario.value.email,
        password: this.miFormulario.value.password
      }
      this.localService.setJsonValue('user',user);
    }else{
      this.localService.clearToken('user');
    }


    this.isLoading = true;

    const { email, password, saveSession } = this.miFormulario.value;
    this.authService
      .login(email, password, saveSession)
      .subscribe((resp: TokenResponse) => {
        if (typeof resp != 'string') {
          this.router.navigateByUrl('/protected');
        } else {
          //TODO mensaje de error
          Swal.fire('Error', resp, 'error');
        }
        this.isLoading = false;
      },()=>{
        Swal.fire('Error', "Credenciales invalidas", 'error');
      });
  }

  loginWithExternalProvider(providerName:string){

    this.authService.initLoginWithExternalProvider(providerName);

  }

}
