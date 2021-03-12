
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
