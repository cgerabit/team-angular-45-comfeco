export interface Event {
  id: number;
  name: string;
  date: Date;
  isActive: boolean
}

export interface Technologies {
  id: number;
  name: string;
  technologyIcon?: string;
  areaId: number
}

export interface Comunity {

id:number;
name:string;
url:string;

}
export interface Sponsor{

  id:number;
  name:string;
  sponsorIcon:string;
}

export interface Area{
  id:number;
  name:string;
  areaIcon:string;
}

export interface ContentCreator{


  profilePicture:string;
  realName:string;

  applicationUserTechnology :applicationUserTechnology[];


}
export interface applicationUserSocalNetworks{
  socialNetworkId:number;
  isPrincipal:boolean;
  url:string;
  socialNetworkIcon:string;
  socialNetworkName:string;

}
export interface applicationUserTechnology{

  id:number;
  name:string;
  technologyIcon:string;
  areaId:number;
  isPrincipal:boolean;
}

export interface UserUpdateDTO{
  profilePicture:File;
  realName:string;
  biography:string;
  specialtyId:number;
  genderId:number;
  countryId:number;
  bornDate:Date;

}

export interface Country{
  id:number;
  name:string;
}


export interface Gender{
  id:number;
  name:string;
}

export interface SocialNetwork{
  id:number;
  hostname:string;
  name:string;
  socialNetworkIcon:string;
}

export interface GroupFilter{
  Name?:string;
  TechnologyId?:number;
}

export interface Group{
  id:number;
  name:string;
  description:string;
  technologyId:number;
  technologyName:string;
  groupImage:string;
}
