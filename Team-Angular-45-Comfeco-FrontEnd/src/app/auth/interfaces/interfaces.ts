
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
  bornDate:Date;
}

export interface UserBadges {

  badgeId:string;
  badgeName:string;
  badgeIcon:string;
  description:string;
  instructions:string;
  getDate:Date;

}
