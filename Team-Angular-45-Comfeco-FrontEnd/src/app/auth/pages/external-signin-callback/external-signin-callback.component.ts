import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute} from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../../environments/environment';
@Component({
  selector: 'app-external-signin-callback',
  template:''
})
export class ExternalSigninCallbackComponent implements OnInit {

  constructor(private router:Router,
              private activatedRoute:ActivatedRoute,
              private authService:AuthService) { }


  ngOnInit(): void {

    this.activatedRoute.queryParams.subscribe(params => {
        if(!params.authcode){
          this.router.navigate(['/auth','login']);
          return;

        }
        this.authService.claimAuthCode(params.authcode,environment.externalLoginTokenPurposeName).then(result=> {

          if(!result){
            this.router.navigate(['/auth','login']);
            return;
          }
          this.router.navigate(['/protected']);
        });

    })

  }

}
