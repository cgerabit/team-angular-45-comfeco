import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-recover',
  templateUrl: './recover.component.html',
  styleUrls: ['./recover.component.scss']
})
export class RecoverComponent implements OnInit {

  miFormulario: FormGroup = this.fb.group({
    email: ['test@test.com', [Validators.required, Validators.email]],
  })

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
  }

  recover(){
    //console.log(this.miFormulario.value);
    //console.log(this.miFormulario.valid);

    //this.authService.validarToken().subscribe(resp=> console.log(resp));

  /* const {email} = this.miFormulario.value;
    this.authService.recover( email )
      .subscribe(resp=>{
        console.log(resp);
        if(resp === true){
          this.router.navigateByUrl('/protected');
        }else{
          //TODO mensaje de error
          Swal.fire('Error', resp, 'error');
        }
      })*/

  }

}
