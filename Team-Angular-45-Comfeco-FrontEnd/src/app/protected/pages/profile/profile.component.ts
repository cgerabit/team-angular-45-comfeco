import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../auth/services/auth.service';
import { UserProfile } from '../../../auth/interfaces/interfaces';
import { Technologies } from '../../interfaces/interfaces';
import { HomepageService } from '../../services/homepage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  ngOnInit(): void {



  }

}
