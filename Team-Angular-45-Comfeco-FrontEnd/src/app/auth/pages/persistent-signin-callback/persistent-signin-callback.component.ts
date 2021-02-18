import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-persistent-signin-callback',
  template: '',
})
export class PersistentSigninCallbackComponent implements OnInit {
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      if (!params.authcode) {
        this.router.navigate(['/auth', 'login']);
        return;
      }
      this.authService
        .claimAuthCode(
          params.authcode,
          environment.persistentLoginTokenPurposeName
        )
        .then((result) => {
          if (!result) {
            this.router.navigate(['/auth', 'login']);
            return;
          }
          if (params.internalUrl) {

            this.router.navigate([decodeURIComponent(params.internalUrl)])
          } else {
            this.router.navigate(['/protected']);
          }
        });
    });
  }
}
