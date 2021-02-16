import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute} from '@angular/router';
import { AuthService } from '../services/auth.service';
@Component({
  selector: 'app-external-signin-callback',
  templateUrl: './external-signin-callback.component.html',
  styleUrls: ['./external-signin-callback.component.css']
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
        this.authService.claimAuthCode(params.authcode).then(result=> {

          if(!result){
            this.router.navigate(['/auth','login']);
            return;
          }
          debugger
          this.router.navigate(['/protected']);

        });

    })

  }

}
