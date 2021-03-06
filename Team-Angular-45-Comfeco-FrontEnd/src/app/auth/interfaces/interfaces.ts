
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


export interface Exam{
  questions:Question[];

}

export interface Question{

  questionText:string;
  answers:Answer[];

}

export interface Answer{

  answerText:string;
  isCorrect:boolean;


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
