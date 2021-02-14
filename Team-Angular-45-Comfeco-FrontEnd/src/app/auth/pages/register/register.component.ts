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
  miFormulario: FormGroup = this.fb.group(
    {
      name: ['test1', [Validators.required]],
      email: ['test1@test1.com', [Validators.required, Validators.email]],
      password: ['123456', [Validators.required, Validators.minLength(6)]],
      password2: ['123456', [Validators.required, Validators.minLength(6)]],
    },
    { validators: this.checkPasswords }
  );

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {}

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

    this.authService.register(name, email, password).subscribe((resp) => {
      console.log(resp);
      if (resp === true) {
        this.router.navigateByUrl('/protected');
      } else {
        //TODO mensaje de error
        Swal.fire('Error', resp, 'error');
      }
    });
  }
}
