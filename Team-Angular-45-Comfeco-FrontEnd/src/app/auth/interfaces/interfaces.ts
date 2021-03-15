import { FlipEffectEvents } from "swiper/types";
import { SocialNetwork } from '../../protected/interfaces/interfaces';

export interface Usuario {
  uid: string;
  name: string;
  email: string;
}

export interface TokenResponse {
  expiration?: Date;
  token?: string;
  responseType?: string;
  isPersistent:boolean;
}


export interface Pagination{


  Page:number;
  RecordsPerPage:number;
}

export interface UserProfile{
  profilePicture:string;
  realName:string;
  userId:string;
  biography:string;
  specialtyId:number;
  genderId:number;
  countryId:number;
  userName:string;
  bornDate:string;
  bornDateParsed:Date;
}

export interface UserBadges {

  badgeId:string;
  badgeName:string;
  badgeIcon:string;
  description:string;
  instructions:string;
  getDate:Date;
}

export interface UserGroup{

  members:GroupMember[];
  groupName:string;
  groupImage:string;
}


export interface GroupMember
{
  name:string;
  profilePicture:string;
  isGroupLeader:boolean;
}

export interface GroupJoinResult {
  alreadyInAgroup:boolean;
  success:boolean;
  alreadyInThisGroup:boolean;
}

export interface changeUsernameDTO{

newUsername:string;
password:string;
userId?:string;

}

export interface changeEmailDTO{

  newEmail:string;
  password:string;
  userId?:string;

  }

  export interface changePasswordDTO{
    currentPassword:string;
    newPassword:string;
    userId?:string;
  }

  export interface updateProfileDTO{
    profilePicture?:File;
    biography:string;
    specialtyId:number;
    genderId:number;
    countryId:number;
    bornDate:string;
  }


  export interface socialNetworkCreationDTO
  {
    socialNetworkId:number;

    url:string;

    isPrincipal?:boolean;
  }


  export interface UserEventInscriptionDTO{

    userId:string;
    eventId:number;
    inscriptionDate:Date;
    isActive:boolean;
    eventName:String;

  }
