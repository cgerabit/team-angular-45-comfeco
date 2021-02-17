
export interface Usuario {
  uid: string;
  name: string;
  email: string;
}

export interface TokenResponse {
  expiration?: Date;
  token?: string;
  responseType?: string;
}
