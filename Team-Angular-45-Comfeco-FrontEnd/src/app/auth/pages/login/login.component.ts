import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import Swal from 'sweetalert2';
import { TokenResponse } from '../../interfaces/interfaces';

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
    remember: [true],
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {}

  change(): void {
    this.isLoading = !this.isLoading;
  }

  login() {
    //console.log(this.miFormulario.value);
    //console.log(this.miFormulario.valid);

    //this.authService.validarToken().subscribe(resp=> console.log(resp));
    this.isLoading = true;

    const { email, password, remember } = this.miFormulario.value;
    this.authService
      .login(email, password, remember)
      .subscribe((resp: TokenResponse) => {
        console.log(typeof resp);
        if (typeof resp != 'string') {
          this.router.navigateByUrl('/protected');
        } else {
          //TODO mensaje de error
          Swal.fire('Error', resp, 'error');
        }
        this.isLoading = false;
      });
  }
}
