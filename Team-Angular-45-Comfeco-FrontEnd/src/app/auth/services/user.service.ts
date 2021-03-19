import { Injectable } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';
import { AuthService } from './auth.service';
import { GroupJoinResult, UserActivityDTO, UserBadges, UserEventInscriptionDTO, UserProfile, updateProfileDTO, socialNetworkCreationDTO, UserGroup } from '../interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';
import { environment } from '../../../environments/environment.prod';
import { applicationUserSocalNetworks, Area } from 'src/app/protected/interfaces/interfaces';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private authService:AuthService,
    private homepageService:HomepageService,
    private http:HttpClient) {

      authService.OnLogout.subscribe(resp=>{
        if(!resp){
          this._userProfile=null;
          this._userSocialNetworks =null;
          this._userSpecialty=null;
          this._userEvents=null;
          this._userActivity=null;
        }

      })

    }
    baseUrl:string = environment.baseUrl;


    //-------------------------GETTERS------------------------------
  private _profileChanged:Subject<UserProfile> =new Subject<UserProfile>();
  public profileChanged:Observable<UserProfile> = this._profileChanged.asObservable();

  private _userProfile:UserProfile

  get userProfile():Promise<UserProfile>
  {

    return new Promise((resolve) =>{

      if(!this.authService.isLoggedIn){
       resolve(null);
       return;
      }
      if(this._userProfile){
        resolve(this._userProfile);
        return;
      }

      this.getUserProfile().subscribe(resp => {


          if(resp.bornDate){
         let splitedDate = resp.bornDate.split('T')[0].split('-');
        resp.bornDateParsed = new Date(
          Number.parseInt(splitedDate[0]),
         Number.parseInt(splitedDate[1])-1
        ,Number.parseInt(splitedDate[2]));
       }

      this._userProfile =resp;

          resolve(resp);

      },() => resolve(null));
    })
  }

  private _socialNetworkChanged:Subject<applicationUserSocalNetworks[]> =new Subject<applicationUserSocalNetworks[]>();
  public socialNetworkChanged:Observable<applicationUserSocalNetworks[]> = this._socialNetworkChanged.asObservable();
  private _userSocialNetworks: applicationUserSocalNetworks[];

  get userSocialNetworks():Promise<applicationUserSocalNetworks[]>
  {
    return new Promise<applicationUserSocalNetworks[]>((resolve)=>{

        if(!this.authService.isLoggedIn){
          resolve(null);
          return;
        }
        if(this._userSocialNetworks){
          resolve(this._userSocialNetworks);
          return;
        }

        this.getUserSocialNetworks().subscribe(resp => {


          this._userSocialNetworks =resp;
          resolve(resp);

        },()=> resolve(null))
    });
  }

  private _userSpecialtyChanged:Subject<Area> =new Subject<Area>();
  public userSpecialtyChanged:Observable<Area> = this._userSpecialtyChanged.asObservable();

  private _userSpecialty : Area

  get userSpecialty():Promise<Area>{
    return new Promise<Area>((resolve) => {
        if(!this.authService.isLoggedIn){
          resolve(null);
          return;
        }
        if(this._userSpecialty){
          resolve(this._userSpecialty);
          return;
        }
          this.userProfile.then(resp => {
              if(!resp || resp.specialtyId ==0){
                resolve(null);
                return;
              }

              this.homepageService.getArea(resp.specialtyId)
              .subscribe(resp => {

                this._userSpecialty =resp;
                resolve(resp);

              },()=>resolve(null))
          })
    })
  }


  get userBadges():Promise<UserBadges[]>{
    return new Promise((resolve)=>{
      if(!this.authService.isLoggedIn){
        resolve(null);
        return;
      }

       this.http.get<UserBadges[]>(`${this.baseUrl}/users/profile/${this.authService.userInfo.userId}/badges`).subscribe(
        resp=>{

          resolve(resp);

        },()=>resolve(null))
    })
  }

  private _userEventsChanged:Subject<UserEventInscriptionDTO[]> =new Subject<UserEventInscriptionDTO[]>();
  userEventsChanged:Observable<UserEventInscriptionDTO[]> = this._userEventsChanged.asObservable();

  private _userEvents:UserEventInscriptionDTO[];
  get userEvents():Promise<UserEventInscriptionDTO[]>{
    return new Promise<UserEventInscriptionDTO[]>(resolve => {
      if(!this.authService.isLoggedIn){
        return resolve(null);
      }

      if(this._userEvents){
        return resolve(this._userEvents);
      }

      this.http.get<UserEventInscriptionDTO[]>(`${this.baseUrl}/events/userevents/${this.authService.userInfo.userId}`)
      .subscribe(r => {
        this._userEvents = r;
        resolve(r);
      },()=>resolve(null))


    })
  }

  private _userActivityChanged:Subject<UserActivityDTO[]>= new Subject<UserActivityDTO[]>();
  userActivityChanged:Observable<UserActivityDTO[]> = this._userActivityChanged.asObservable();

  private _userActivity:UserActivityDTO[];
  get userActivity():Promise<UserActivityDTO[]>{
    return new Promise<UserActivityDTO[]>((resolve)=> {
      if(!this.authService.isLoggedIn){
        return resolve(null);
      }

      if(this._userActivity){
        return resolve(this._userActivity);
      }
      this.http.get<UserActivityDTO[]>(`${this.baseUrl}/users/profile/${this.authService.userInfo.userId}/activity`).subscribe(r=> {
        this._userActivity = r;
        resolve(r);

      },()=>resolve(null));

    })
  }

  //---------------------------------------- END GETTERS ----------------------------



  getUserProfile() : Observable<UserProfile>{

    if(!this.authService.userInfo || !this.authService.userInfo.userId){
      return of(null);
    }

    const url = `${this.baseUrl}/users/profile/${this.authService.userInfo.userId}`;


      return this.http.get<UserProfile>(url);

  }

  getUserSocialNetworks(){
    if(!this.authService.userInfo || !this.authService.userInfo.userId){
      return of(null);
    }

    const url = `${this.baseUrl}/users/profile/${this.authService.userInfo.userId}/socialnetworks`;

    return this.http.get<applicationUserSocalNetworks[]>(url);

  }

  leaveUserFromGroup(){
    return new Promise<boolean>(resolve=>{


      if(!this.authService.isLoggedIn){
        return resolve(false);
      }

      this.http.delete(`${this.baseUrl}/groups/members/${this.authService.userInfo.userId}`)
      .subscribe(() => {
        this._userActivity=null;
        this.userActivity.then(r=>{
          this._userActivityChanged.next(r);
        })
          resolve(true);

      },()=>resolve(false))

    })

  }

  addUserInAGroup(groupId:number){
    return new Promise<GroupJoinResult>(resolve=>{

      let failureResult = {
        success:false,
        alreadyInAgroup:false,
        alreadyInThisGroup:false
      };
      if(!this.authService.isLoggedIn){
        return resolve(failureResult);

      }

      this.http.post<GroupJoinResult>(`${this.baseUrl}/groups/members/${groupId}`
      ,{userId:this.authService.userInfo.userId}).subscribe(r => {

        this._userActivity=null;
        this.userActivity.then(r=>{
          this._userActivityChanged.next(r);
        })

          resolve(r);
          return;

      }, ()=>{
          return resolve(failureResult);
      });

    })
  }


  updateProfile(updateProfileDTO:updateProfileDTO){
    var formData:FormData=new FormData();
    if(updateProfileDTO.profilePicture){
     formData.append('profilePicture',updateProfileDTO.profilePicture);
    }
    formData.append('bornDate',updateProfileDTO.bornDate);
    formData.append('biography',updateProfileDTO.biography);
    formData.append('countryId',updateProfileDTO.countryId.toString());
    formData.append('genderId',updateProfileDTO.genderId.toString());
    formData.append('specialtyId',updateProfileDTO.specialtyId.toString())

    return this.http.put(`${this.baseUrl}/users/profile/${this.authService.userInfo.userId}`,formData)
    .pipe(
      map(r => {
       //delete cache
       this._userProfile =null;
       this._userSpecialty =null;
       this.userProfile.then(p => {
         this._profileChanged.next(p);
       });

       this.userSpecialty.then(p => {
         this._userSpecialtyChanged.next(p);
       })


       return r;
      })
    )

   }

   updateSocialNetworks(socialNetworkCreationDTO:socialNetworkCreationDTO[]){

       return this.http.post(`${this.baseUrl}/users/profile/${this.authService.userInfo.userId}/fillsocialnetworks`,socialNetworkCreationDTO)
       .pipe(map(r=>{
         //delete cache
         this._userSocialNetworks=null;
         this.userSocialNetworks.then(r=> {
          this._socialNetworkChanged.next(r);
         }).catch();

         return r;
       }));

   }

   addUserToEvent(eventId:number, userId:string){

     const addUserDTO = {
       userId
     }
     return this.http.post(`${this.baseUrl}/events/${eventId}/adduser`,addUserDTO)
     .pipe(map(r =>{
       this._userEvents=null;
       this._userActivity=null;
       this.userEvents.then(r=>{

         this._userEventsChanged.next(r);
       })
       this.userActivity.then(r=>{
         this._userActivityChanged.next(r);
       })

       return r;
     }));
   }

   removeUserFromEvent(eventId:number,userId:string){

     return this.http
     .delete(`${this.baseUrl}/events/${eventId}/removeuser/${userId}`)
     .pipe(map(r =>{
       this._userEvents=null;
       this._userActivity=null;
       this.userEvents.then(r=>{

         this._userEventsChanged.next(r);
       })
       this.userActivity.then(r=>{
         this._userActivityChanged.next(r);
       })

       return r;
     }));
   }

   currentUserGroup:UserGroup
   getUserGroup(useCache:boolean=true):Promise<UserGroup>{
     return new Promise<UserGroup>((resolve)=>{

         if(!this.authService.isLoggedIn){
           resolve(null);
           return;
         }

         if(this.currentUserGroup && useCache){
             resolve(this.currentUserGroup);
             return;
         }

         this.http.get<UserGroup>(`${this.baseUrl}/groups/members/usergroup/${this.authService.userInfo.userId}`)
         .subscribe(resp => {

           this.currentUserGroup = resp;
           resolve(resp);

         },()=>resolve(null))

     });
   }


}
