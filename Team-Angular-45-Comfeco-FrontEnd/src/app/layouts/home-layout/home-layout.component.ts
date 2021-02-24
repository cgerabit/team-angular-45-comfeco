import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-home-layout',
  templateUrl: './home-layout.component.html',
  styleUrls: ['./home-layout.component.scss']
})
export class HomeLayoutComponent implements OnInit {

  constructor(private router: Router,
    private authService: AuthService) { }

  get usuario(){
    return this.authService.userInfo
  }
  ngOnInit(): void {
  }
  logout(){
    this.router.navigateByUrl('/auth/login');
    this.authService.logout();
  }

}
