import { Component, OnInit } from '@angular/core';
import { UserProfile } from 'src/app/auth/interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { AuthService } from '../../../../auth/services/auth.service';
import { Technologies, Area, applicationUserSocalNetworks, Country, Gender, SocialNetwork } from '../../../interfaces/interfaces';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { debug } from 'console';
import { UserBadges } from '../../../../auth/interfaces/interfaces';

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
    private fb: FormBuilder,
   private homeService:HomepageService) { }

  profile:UserProfile;
  userSpecialty:Area;

  countries:Country[]=[];

  genders:Gender[]=[];

  areas:Area[]=[];

  badges:UserBadges[]=[];

  userSocialNetworks:applicationUserSocalNetworks[];

  form:FormGroup;

  socialNetworks:SocialNetwork[] = [];


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


    this.loadData();
    this.form = this.fb.group(
      {
        biography:['',{validators:[Validators.required]}],
        specialtyId:[1,{Validators:[Validators.required]}],
        genderId:[1,{Validators:[Validators.required]}],
        countryId:[1,{Validators:[Validators.required]}],
        bornDate:[1,{Validators:[Validators.required]}],
        email:[''],
        facebook:[''],
        twitter:[''],
        linkedin:[''],
        github:['']
      }
    );

  }

  loadData(){

    this.authService.userProfile.then(r => {

      this.profile =r;
      this.form.get('biography').setValue(this.profile.biography);
      this.form.get('specialtyId').setValue(this.profile.specialtyId);
      this.form.get('countryId').setValue(this.profile.countryId);
      this.form.get('genderId').setValue(this.profile.genderId);
      this.form.get('bornDate').setValue(this.profile.bornDate);

    } );
    this.authService.userSocialNetworks.then(r=> {
      this.userSocialNetworks = r;

      let facebookO = r.find(s => s.socialNetworkName.toLowerCase()=="facebook")
      if(facebookO){
        this.form.get('facebook').setValue(facebookO.url)
      }
      let linkedin = r.find(s => s.socialNetworkName.toLowerCase() == "linkedin");
      if(linkedin){
        this.form.get('linkedin').setValue(linkedin.url);
      }
      let twitter = r.find(s=> s.socialNetworkName.toLowerCase() == "twitter");
      if(twitter){

        this.form.get('twitter').setValue(twitter.url)
      }
      let github = r.find(s=> s.socialNetworkName.toLowerCase() =="github");
      if(github){
        this.form.get('github').setValue(github.url)
      }


    } );
    this.authService.userSpecialty.then(r => this.userSpecialty = r );

    this.authService.userBadges.then(r=> this.badges = r);

    this.homeService.getAreas({Page:1,RecordsPerPage:150})
    .subscribe(resp => {
      this.areas = resp;
    },()=>console.log("Error loading areas"));

    this.homeService.getGenders().subscribe(genders => {
      console.log(genders);
        this.genders = genders;
        console.log(this.genders);

    },()=> console.log("Error loading generos"));

    this.homeService.getCountries().subscribe(countries => {
      this.countries=countries;
    }, ()=> console.log("Error cargando continentes"))

    this.homeService.getSocialNetworks().subscribe(socialNetworks=> {
      this.socialNetworks = socialNetworks;
    },()=>console.log("Error cargando social networks") )

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
    console.log(this.form.value);
  }

  facebookData:SocialNetwork;
  getFacebookData(){

    if(!this.facebookData){
     this.facebookData=this.socialNetworks.find(s => s.name.toLowerCase()=="facebook");
    }

    return this.facebookData;
  }
  twitterData:SocialNetwork;
  getTwitterData(){
    if(!this.twitterData){
      this.twitterData=this.socialNetworks.find(s=> s.name.toLowerCase() == "twitter");
    }

    return this.twitterData;
  }
  linkedinData:SocialNetwork;
  getLinkedinData(){

    if(!this.linkedinData){
      this.linkedinData=this.socialNetworks.find(s=> s.name.toLowerCase() == "linkedin");
    }
    return this.linkedinData;
  }
  githubData:SocialNetwork;
  getGithubData(){
    if(!this.githubData){
      this.githubData=this.socialNetworks.find(s=> s.name.toLowerCase()=="github");
    }
    return this.githubData;
  }
}
