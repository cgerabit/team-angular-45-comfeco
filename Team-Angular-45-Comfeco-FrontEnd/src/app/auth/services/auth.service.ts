import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { catchError, map, tap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import * as shajs from 'sha.js'
import { environment } from '../../../environments/environment';
import {  TokenResponse, Usuario } from '../interfaces/interfaces';
import { claimAuthCodeDTO } from '../DTOs/claimAuthCodeDTO';
import { generateRandomString } from '../../../utils/Utilities';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: String = environment.baseUrl;
  private _usuario: Usuario;

 // =========================================================
 //                          GETTERS
// =========================================================
  get usuario() {
    return { ...this._usuario };
  }
//TOKEN
  private _token:string="";

  get token():string{
    if(!this._token){
      this._token = localStorage.getItem(environment.tokenFieldName);
    }
    return this._token;
  }

  set token(value:string){
    localStorage.setItem(environment.tokenFieldName,value);
    this._token = value;
  }
  //TOKEN EXPIRATION
  private _tokenExpiration:Date;
  get tokenExpiration():Date{
    if(!this._tokenExpiration){
      const tokenExpirationFromStorage = localStorage.getItem(environment.tokenExpirationFieldName);
      this._tokenExpiration= new Date(tokenExpirationFromStorage);
    }
    return this._tokenExpiration;
  }
  set tokenExpiration(value:Date){
    localStorage.setItem(environment.tokenExpirationFieldName,value.toString());
    this._tokenExpiration=value;
  }
  get isLoggedIn():boolean{
    //TODO:
    //chequear token con el servidor

    return  this.token && !this.isTokenExpired;

  }
  get isTokenExpired():boolean {

    return (this.tokenExpiration &&  new Date() >= this.tokenExpiration) || !this.tokenExpiration

  }
  //---------------------------------------- END GETTERS ----------------------------

  constructor(private http: HttpClient) {}



  register(name: string, email: string, password: string) {
    const url = `${this.baseUrl}/Account/register`;
    const body = { email: email, password: password, userName: name };

    return this.http.post(url, body).pipe(
      tap((resp) => {
        console.log(resp);
      }),
      catchError((err) => of(err.error))
    );
  }

  login(email: string, password: string, persistLogin: boolean) {
    const url = `${this.baseUrl}/Account/login`;
    const body = {
      userName: email,
      password: password,
      persistLogin: persistLogin,
    };

    return this.http.post<TokenResponse>(url, body).pipe(
      tap((resp) => {
        this.authenticateUser(resp);

      }),
      catchError((err: HttpErrorResponse) => of(err.error))
    );
  }

  private authenticateUser ( tokenResp : TokenResponse){

    debugger
    if(!tokenResp || !tokenResp.responseType){
      return;
    }

    if(tokenResp.responseType==environment.bearertokenResponseTypeName)
    {
      this.token= tokenResp.token;
      this.tokenExpiration = tokenResp.expiration;
    }

  }

  initLoginWithExternalProvider(providerName:string,
    returnUrl:string=environment.externalLoginEndpoint)
  {
    let securityKey = generateRandomString(15);
    // save original key
  localStorage.setItem(environment.externalLoginSecurityKeyFieldName,securityKey);
    // generate sha256
    let securityKeyHash = shajs('sha256').update(securityKey).digest('hex')
    debugger
    let url =
    `${this.baseUrl}/account/externallogin?SecurityKeyHash=${encodeURIComponent(btoa(securityKeyHash))}&Provider=${providerName}&returnUrl=${encodeURIComponent(returnUrl)}`


    location.href=url;

  }

  claimAuthCode(authCode:string):Promise<boolean>
  {
      return new Promise((resolve)=>{

        let securityKey = localStorage.getItem(environment.externalLoginSecurityKeyFieldName);
        if(!securityKey || !authCode){
           resolve(false);
          return;
        }

        const body:claimAuthCodeDTO =
        {
          token:authCode,
          securityKey:btoa(securityKey)
        }
        debugger
        this.http.post<TokenResponse>(`${this.baseUrl}/account/claimexternal`,body).subscribe(resp => {
          debugger
          this.authenticateUser(resp);

          resolve(true)
        },() =>resolve(false))

      });
  }



  logout() {
    //TODO:
    //ENVIAR LOGOUT AL SERVIDOR
    localStorage.removeItem(environment.tokenFieldName);
    localStorage.removeItem(environment.tokenExpirationFieldName);
  }

  tryRenewToken():Promise<boolean>{
    return new Promise<boolean>((resolve)=>{
      if(!this.isLoggedIn){
        resolve(false);
        return;
      }
    let renovationTime=new Date();
    //Si el token le quedan menos de 35 minutos de vida
    renovationTime.setMinutes(renovationTime.getMinutes() + 35 );
    if(renovationTime < this.tokenExpiration){
      //No necesitamos renovar todavia
      resolve(false);
      return;
    }
      this.http.get<TokenResponse>(`${this.baseUrl}/account/refreshtoken`)
      .subscribe(resp => {
        this.authenticateUser(resp);
        resolve(true);
      },()=>{
        this.logout();
        resolve(false);
      })
    });
}
}
