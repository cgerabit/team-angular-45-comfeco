import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  isLoading: boolean = false;
  miFormulario: FormGroup = this.fb.group(
    {
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.pattern(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]],
      password: ['', [Validators.required, Validators.pattern(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$/)]],
      password2: ['', [Validators.required, Validators.minLength(6)]],
    },
    { validators: this.checkPasswords }
  );

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {}

  //validaciones
  get nombreNoValido(){
    return this.miFormulario.get('name').invalid && this.miFormulario.get('name').touched
  }
  get correoNoValido(){
    return this.miFormulario.get('email').invalid && this.miFormulario.get('email').touched
  }
  get passwordNoValido(){
    return this.miFormulario.get('password').invalid && this.miFormulario.get('password').touched
  }
  get password2NoValido(){
    const password1 = this.miFormulario.get('password').value;
    const password2 = this.miFormulario.get('password2').value;

    return (password1 == password2) ? false: true;
  }

  checkPasswords(group: FormGroup) {
    // here we have the 'passwords' group
    const password = group.get('password').value;
    const password2 = group.get('password2').value;

    return password === password2 ? null : { notSame: true };
  }

  register() {
    //console.log(this.miFormulario.value);
    //console.log(this.miFormulario.valid);
    const { name, email, password } = this.miFormulario.value;
    this.isLoading = true;
    this.authService.register(name, email, password).subscribe((resp) => {
      if (resp === null) {
        Swal.fire(
          'Todo correcto',
          'Revisa tu email para confirmar tu cuenta',
          'success'
        );
      } else {
        Swal.fire('Error', resp[0], 'error');
      }
      this.isLoading = false;
    });
  }
  registerWithExternalProvider(providerName:string){

    this.authService.initLoginWithExternalProvider(providerName);

  }
}
