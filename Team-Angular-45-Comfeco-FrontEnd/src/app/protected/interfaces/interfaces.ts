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

export interface ContentCreator{


  profilePicture:string,
  realName:string,

  applicationUserTechnology :applicationUserTechnology[];


}

export interface applicationUserTechnology{

  id:number;
  name:string;
  technologyIcon:string;
  areaId:number;
  isPrincipal:boolean;
}


