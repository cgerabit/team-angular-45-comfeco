import { Component, OnInit } from '@angular/core';
import { UserProfile } from 'src/app/auth/interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { AuthService } from '../../../../auth/services/auth.service';
import { Technologies, Area, applicationUserSocalNetworks } from '../../../interfaces/interfaces';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-tab-profile',
  templateUrl: './tab-profile.component.html',
  styleUrls: ['./tab-profile.component.scss']
})
export class TabProfileComponent implements OnInit {

  public slidesSponsor = [
    'https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/Angular_full_color_logo.svg/1200px-Angular_full_color_logo.svg.png',
    'https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/React-icon.svg/1200px-React-icon.svg.png',
    'https://www.manejandodatos.es/wp-content/uploads/2018/02/vueJS.png',
    'https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Svelte_Logo.svg/1200px-Svelte_Logo.svg.png',
  ];

  //cambiar componente visualizado
  updateState: boolean = false;
  model: NgbDateStruct;

  //imagen del usuario
  imageURL: string = this.usuario.avatar;

  //FORMULARIO
  uploadForm: FormGroup = this.fb.group({
    avatar: [null],
    email: [
      '',
      [
        Validators.required,
        Validators.pattern(
          /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
        ),
      ],
    ],
    date: [''],
  });


  constructor(private authService:AuthService,
    private fb: FormBuilder) { }

  profile:UserProfile;
  userSpecialty:Area;

  userSocialNetworks:applicationUserSocalNetworks[];



  get usuario(){
    const { userName } = this.authService.userInfo;
    return {
      userName: userName,
      avatar:
        `https://avatars.dicebear.com/api/bottts/${userName}.svg`,
    };
  }

  //cambiar componente visualizado
  update() {
    this.updateState = !this.updateState;
  }

  ngOnInit(): void {

    this.authService.userProfile.then(r => this.profile = r );
    this.authService.userSocialNetworks.then(r=> this.userSocialNetworks = r );
    this.authService.userSpecialty.then(r => this.userSpecialty = r );

  }

  // Image Preview
  showPreview(event) {
    const file = (event.target as HTMLInputElement).files[0];
    this.uploadForm.patchValue({
      avatar: file,
    });
    this.uploadForm.get('avatar').updateValueAndValidity();

    // File Preview
    const reader = new FileReader();
    reader.onload = () => {
      this.imageURL = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  // Submit Form
  submit() {
    console.log(this.uploadForm.value);
  }

}
