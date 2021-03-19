import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { UserProfile } from 'src/app/auth/interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { AuthService } from '../../../../auth/services/auth.service';
import {  Area, applicationUserSocalNetworks, Country, Gender, SocialNetwork, ActiveEvent } from '../../../interfaces/interfaces';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserBadges, socialNetworkCreationDTO, UserEventInscriptionDTO, UserActivityDTO, UserProviders } from '../../../../auth/interfaces/interfaces';
import { ChangeComponent } from 'src/app/protected/components/change/change.component';
import Swal from 'sweetalert2';
import { UserService } from '../../../../auth/services/user.service';
import { LoadingOverlayService } from '../../../../shared/services/loading-overlay.service';
import { Observable,merge} from 'rxjs';

@Component({
  selector: 'app-tab-profile',
  templateUrl: './tab-profile.component.html',
  styleUrls: ['./tab-profile.component.scss']
})
export class TabProfileComponent implements OnInit {


  //cambiar componente visualizado
  updateState: boolean = false;
  model: NgbDateStruct;

  //imagen del usuario
  imageURL: string = null


  constructor(public authService:AuthService,
    private fb: FormBuilder,
    private userService:UserService,
   private homeService:HomepageService,
   private modalService: NgbModal,
   private loadingOverlay:LoadingOverlayService,
) {

  userService.socialNetworkChanged.subscribe(r=>{
      this.userSocialNetworks = r;
  })
 }

  profile:UserProfile;
  userSpecialty:Area;

  countries:Country[]=[];

  genders:Gender[]=[];

  areas:Area[]=[];

  badges:UserBadges[]=[];

  userSocialNetworks:applicationUserSocalNetworks[];

  form:FormGroup;

  socialNetworks:SocialNetwork[] = [];


  userProviders:UserProviders={
    haveFacebook:false,
    haveGoogle:false
  };
  userEvents:UserEventInscriptionDTO[] = [];

  userActivities:UserActivityDTO[] = [];

  @Output()
  eventosEmitter:EventEmitter<object> = new EventEmitter();

  get usuario(){
    const { userName, email } = this.authService.userInfo;
    return {
      userName: userName,
      email: email,
      avatar:
        `https://avatars.dicebear.com/api/bottts/${userName}.svg`,
    };
  }



  //cambiar componente visualizado
  update() {
    this.updateState = !this.updateState;
  }

   //Abrir modal
   open(label: string, isPassword: boolean, inputData: string) {
    const modalRef = this.modalService.open(ChangeComponent, { centered: true });
    modalRef.componentInstance.dataModal = {
      label,
      isPassword,
      inputData,
    };
  }


  ngOnInit(): void {


    this.loadData();
    this.form = this.fb.group(
      {
        biography:['',{validators:[Validators.required, Validators.maxLength(140)]}],
        specialtyId:[1,{Validators:[Validators.required]}],
        genderId:[1,{Validators:[Validators.required]}],
        countryId:[1,{Validators:[Validators.required]}],
        bornDate:[1,{Validators:[Validators.required]}],
        email:[''],
        facebook:[''],
        twitter:[''],
        linkedin:[''],
        github:[''],
        profilePicture:[null]
      }
    );

    this.userService.profileChanged.subscribe(p=> {
        this.profile = p;

    });

    this.userService.userSpecialtyChanged.subscribe(s => {
      this.userSpecialty = s;
    })

    this.userService.userEventsChanged.subscribe(r=> {
      this.userEvents = r;
    })

    this.userService.userActivityChanged.subscribe(r=>{
      this.userActivities= r;
    })
  }


  loadData(){


    this.loadingOverlay
    .setTimerWith(this.userService.userEvents).then(r=>{
      console.log(r)
      this.userEvents =r ;
    })

    this.loadingOverlay
    .setTimerWith(this.userService.userActivity).then(resp=> {
      this.userActivities =resp;
    });


    this.loadingOverlay.setTimerWith(this.userService.userProfile).then(r => {

      this.profile = r;
      this.imageURL = r.profilePicture || this.usuario.avatar;
      this.form.get('biography').setValue(this.profile.biography);
      this.form.get('specialtyId').setValue(this.profile.specialtyId);
      this.form.get('countryId').setValue(this.profile.countryId);
      this.form.get('genderId').setValue(this.profile.genderId);
     if(r.bornDate){
        let day = r.bornDateParsed.getDate();
        let month = r.bornDateParsed.getMonth()+1;
        let year = r.bornDateParsed.getFullYear();
        this.model = {day,month,year};
        this.form.get('bornDate').setValue(this.model);
      }
    } );

    this.loadingOverlay.setTimerWith(this.userService.userSocialNetworks).then(r=> {
      this.userSocialNetworks = r;

      let facebookO = r.find(s => s.socialNetworkName.toLowerCase()=="facebook")
      if(facebookO){
        this.form.get('facebook').setValue(facebookO.url.replace("https://www.facebook.com/",""))
      }
      let linkedin = r.find(s => s.socialNetworkName.toLowerCase() == "linkedin");
      if(linkedin){
        this.form.get('linkedin').setValue(linkedin.url.replace("https://www.linkedin.com/","")
        .replace("in/","").replace("/",""));
      }
      let twitter = r.find(s=> s.socialNetworkName.toLowerCase() == "twitter");
      if(twitter){

        this.form.get('twitter').setValue(twitter.url.replace("https://twitter.com/",""))
      }
      let github = r.find(s=> s.socialNetworkName.toLowerCase() =="github");
      if(github){
        this.form.get('github').setValue(github.url.replace("https://github.com/",""))
      }


    } )


    this.loadingOverlay.setTimerWith(this.userService.userSpecialty).then(r => this.userSpecialty = r );


    this.loadingOverlay.setTimerWith(this.userService.userBadges.then(r=> this.badges = r));
    console.log(this.badges);




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

    this.authService.userProviders().subscribe(r=> {
      this.userProviders=r;
    })
  }
  // Image Preview
  showPreview(event) {
    const file = (event.target as HTMLInputElement).files[0];
    this.form.patchValue({
      profilePicture: file,
    });

    // File Preview
    const reader = new FileReader();
    reader.onload = () => {
      this.imageURL = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  // Submit Form
  submit() {

    this.form.markAllAsTouched();

    if(!this.form.valid || this.loadingOverlay.loadingOverlayVisible){
        return;
    }
    let date:NgbDateStruct =this.form.get('bornDate').value;

    let updateProfileObs =this.userService.updateProfile({
      genderId:this.form.get('genderId').value,
      biography:this.form.get('biography').value,
      bornDate:`${date.year}-${date.month}-${date.day}`,
      countryId:this.form.get('countryId').value,
      specialtyId:this.form.get('specialtyId').value,
      profilePicture:this.form.get('profilePicture').value

     });




    let socialNetworkArray:socialNetworkCreationDTO[] = [];
    let twitterField= this.form.get("twitter");
    if(twitterField.dirty && twitterField.value){
      socialNetworkArray.push({
        url:`https://twitter.com/${twitterField.value}`,
        socialNetworkId:this.getTwitterData().id
      });
    }

    let linkedinField= this.form.get('linkedin')
    if(linkedinField.dirty && linkedinField.value){
      socialNetworkArray.push({
        url:`https://www.linkedin.com/in/${linkedinField.value}`,
        socialNetworkId:this.getLinkedinData().id
      })
    }
    let githubField = this.form.get('github');
    if(githubField.dirty && githubField.value){
      socialNetworkArray.push({
        url:`https://github.com/${githubField.value}`,
        socialNetworkId:this.getGithubData().id
      })
    }
    let facebookField = this.form.get('facebook');
    if(facebookField.dirty && facebookField.value){
      socialNetworkArray.push({
        url:`https://www.facebook.com/${facebookField.value}`,
        socialNetworkId:this.getFacebookData().id
      });
    }
    if(socialNetworkArray.length>0){
       this.userService.updateSocialNetworks(socialNetworkArray).subscribe(()=>{

      },()=>{

      })
    }



    this.loadingOverlay.setTimerWith(updateProfileObs).then(()=>{
      Swal.fire({
        title:'Exito!',
        text:'Has actualizado tus datos exitosamente',
        icon:'success'
      })
      this.update();
    }).catch(()=>{
      Swal.fire({
        title:'Error!',
        text:'Ha ocurrido un error actualizando los datos',
        icon:'error'
      })
    })

    //this.authService.updateSocialNetworks()

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
  leaveFromEvent(event:UserEventInscriptionDTO){
    Swal.fire({
      title: '¿Estas seguro?',
      text: `¿Estas seguro que quieres salir del evento ${event.eventName}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#F2A007',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si quiero salir'
    }).then(r =>{
      if(r.isConfirmed){
        this.loadingOverlay.setTimerWith(this.userService.
        removeUserFromEvent(event.eventId,this.authService.userInfo.userId)).then(()=>{
          Swal.fire({
            title:"Exito!",
            text:"Has abandonado correctamente el evento",
            icon:"success"
          })
        }).catch(()=>{
            Swal.fire({
              title:"Error!",
              text:"Ha ocurrido un error inesperado",
              icon:"error"
            })
        });
      }
    })
  }

  moveToEvents(){
    this.eventosEmitter.next();
  }


  initExternalProviderLink(providerName:string){
    let obs:Observable<object>;
    if(providerName == "Facebook"){

      if(this.userProviders.haveFacebook){
          obs = this.authService.removeExternalProviderLink(providerName);
      }
      else{
       this.authService.initExternalProviderLink(providerName);
      }
    }
    else if(providerName=="Google"){
      if(this.userProviders.haveGoogle){
         obs = this.authService.removeExternalProviderLink(providerName);
      }
      else{
        this.authService.initExternalProviderLink(providerName);
      }
    }

    if(obs){
      Swal.fire({
        title:'Atencion!',
        text:`Vas a desvincular el proveedor de identidad ${providerName} de tu cuenta, esta accion es irreversible ¿Estas Seguro?`,
        icon:'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, quiero desvincularlo'
      }).then( resp => {
        if(resp.isConfirmed){
        this.loadingOverlay.setTimerWith(obs).then(()=>{
          this.loadingOverlay.setTimerWith(this.authService.userProviders()).then(r =>{
            this.userProviders = r;
          } ).catch().finally(()=> {
            Swal.fire({
              title:'Exito!',
              text:`Has desvinculado el proveedor de identidad ${providerName} de tu cuenta`,
              icon:'success'
            })
          })
        })
        .catch((err)=>{
          let text;

          if(err.error.noPassword){
            text= "Necesitas establecer una contraseña antes de hacer eso";

          }else{
            text = "Ha ocurrido un error inesperado intentalo mas tarde";
          }
          Swal.fire({
            title:'Error!',
            text,
            icon:'error'
          })

        })
        }
      })


    }

  }

}
