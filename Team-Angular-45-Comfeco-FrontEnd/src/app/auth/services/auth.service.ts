import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';

import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import * as shajs from 'sha.js';
import { environment } from '../../../environments/environment';
import { TokenResponse, Usuario, UserProfile, UserBadges } from '../interfaces/interfaces';
import { claimAuthCodeDTO } from '../DTOs/claimAuthCodeDTO';

import {
  generateRandomString,
  setOrDeleteFromStorage,
} from '../../../utils/Utilities';
import { userInfo } from '../interfaces/userInfo';
import { Router } from '@angular/router';

import { identifierModuleUrl } from '@angular/compiler';
import { applicationUserSocalNetworks } from 'src/app/protected/interfaces/interfaces';
import { Technologies, Area } from '../../protected/interfaces/interfaces';
import { HomepageService } from 'src/app/protected/services/homepage.service';


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
  private _token: string = '';

  get token(): string {
    if (!this._token) {
      this._token = localStorage.getItem(environment.tokenFieldName);
    }
    return this._token;
  }

  set token(value: string) {
    setOrDeleteFromStorage(environment.tokenFieldName, value);

    this._token = value;
  }
  //TOKEN EXPIRATION
  private _tokenExpiration: Date;
  get tokenExpiration(): Date {
    if (!this._tokenExpiration) {
      const tokenExpirationFromStorage = localStorage.getItem(
        environment.tokenExpirationFieldName
      );
      this._tokenExpiration = new Date(tokenExpirationFromStorage);
    }
    return this._tokenExpiration;
  }
  set tokenExpiration(value: Date) {
    setOrDeleteFromStorage(
      environment.tokenExpirationFieldName,
      value?.toString()
    );

    this._tokenExpiration = value;
  }
  get isLoggedIn(): boolean {
    //TODO:
    //chequear token con el servidor

    return this.token && !this.isTokenExpired;
  }
  get isTokenExpired(): boolean {
    return (
      (this.tokenExpiration && new Date() >= this.tokenExpiration) ||
      !this.tokenExpiration
    );
  }


  get userHavePersistLogin(): Promise<boolean> {
    return this.checkPersistLogin();
  }

  private _userInfo: userInfo;

  get userInfo(): userInfo {
    if (!this.isLoggedIn) {
      return null;
    }

    if (!this._userInfo) {
      this._userInfo = this.tryDecodeTokenClaims();
    }

    return this._userInfo;
  }

  private _userProfile:UserProfile

  get userProfile():Promise<UserProfile>
  {

    return new Promise((resolve) =>{

      if(!this.isLoggedIn){
       resolve(null);
       return;
      }
      if(this._userProfile){
        resolve(this._userProfile);
        return;
      }

      this.getUserProfile().subscribe(resp => {
          this._userProfile =resp;

          resolve(resp);

      },() => resolve(null));
    })
  }

  private _userSocialNetworks: applicationUserSocalNetworks[];

  get userSocialNetworks():Promise<applicationUserSocalNetworks[]>
  {
    return new Promise<applicationUserSocalNetworks[]>((resolve)=>{

        if(!this.isLoggedIn){
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

  private _userSpecialty : Area

  get userSpecialty():Promise<Area>{
    return new Promise<Area>((resolve) => {
        if(!this.isLoggedIn){
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

  private _userBadges:UserBadges[];
  get userBadges():Promise<UserBadges[]>{
    return new Promise((resolve)=>{
      if(!this.isLoggedIn){
        resolve(null);
        return;
      }

      if(this._userBadges){
        resolve(this._userBadges);
        return;
      }
      return this.http.get<UserBadges[]>(`${this.baseUrl}/users/profile/${this.userInfo.userId}/badges`).subscribe(
        resp=>{

          this._userBadges=resp;
          resolve(resp);
          
        },()=>resolve(null))
    })
  }
  //---------------------------------------- END GETTERS ----------------------------

  constructor(private http: HttpClient,
    private homepageService:HomepageService) {}

  private _userHavePersistLogin: number;
  private async checkPersistLogin(){

    if(this._userHavePersistLogin){
        return this._userHavePersistLogin ===1;
    }
    const persistentStorageValue = localStorage.getItem(
      environment.isPersistentKeyFieldName
    );
    if(!persistentStorageValue || parseInt(persistentStorageValue) !== 1 ){
      return false;
    }
    let persistValue = 0;
    try{
       await this.http
      .get(`${this.baseUrl}/account/havepersistlogin`, {
        withCredentials: true,
      }).toPromise();
      persistValue=1;
    }
    catch(Exception){}
    this._userHavePersistLogin = persistValue;
      localStorage.setItem(environment.isPersistentKeyFieldName, persistValue.toString());
      return this._userHavePersistLogin ===1;
  }

  private async _checkSessionRecover(pathname?: string) {
    if (!this.isLoggedIn) {
      const result = await this.userHavePersistLogin;
      localStorage.setItem(environment.isPersistentKeyFieldName, '0');
      this._userHavePersistLogin = 0;
      if (result) {
        this.initPersistentLogin(
          environment.persistLoginEndpoint,
          pathname ?? location.pathname
        );
      }
    }
  }

  public async checkSessionRecover(pathname?: string) {
    try {
      await this._checkSessionRecover(pathname);
    } catch (exception) {}
  }

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

  recover(email: string){
    const url = `${this.baseUrl}/Account/sendrecoverpwd`;
    const body = {
      email: email
    }
    return this.http.post<any>(url, body).pipe(
      tap( (resp) => {
        console.log("FROM SERVICE", resp)
      }),
      catchError((err: HttpErrorResponse) => of(err.error))
    );

  }

  cambiarClave(user_id: string, token: string, nueva_clave: string){
    const url = `${this.baseUrl}/Account/confirmrecoverpwd`;
    const body = {
      userId: user_id,
      token: `${encodeURIComponent(token)}`,
      password: nueva_clave
    }

    return this.http.post<any>(url, body).pipe(
      tap( (resp) => {
        console.log("FROM SERVICE", resp)
      }),
      catchError((err: HttpErrorResponse) => of(err.error))
    );


  }

  login(email: string, password: string, persistLogin: boolean) {
    const url = `${this.baseUrl}/Account/login`;
    const body = {
      userName: email,
      password: password,
      persistLogin: persistLogin,
    };

    return this.http
      .post<TokenResponse>(url, body, { withCredentials: true })
      .pipe(
        tap((resp) => {
          this.authenticateUser(resp);
          console.log(resp);

        }),
        catchError((err: HttpErrorResponse) => of(err.error))
      );
  }

  private authenticateUser(tokenResp: TokenResponse) {
    if (!tokenResp || !tokenResp.responseType) {
      return;
    }

    if (tokenResp.responseType == environment.bearertokenResponseTypeName) {
      this.token = tokenResp.token;
      this.tokenExpiration = tokenResp.expiration;
      let persistentStorageValue = tokenResp.isPersistent ? 1 : 0;
      localStorage.setItem(
        environment.isPersistentKeyFieldName,
        persistentStorageValue.toString()
      );
      this._userInfo = this.tryDecodeTokenClaims();
    }
  }

  initLoginWithExternalProvider(
    providerName: string,
    returnUrl: string = environment.externalLoginEndpoint
  ) {
    this.initRedirectionLogin(returnUrl, providerName);
  }
  redirectInited: boolean;
  initPersistentLogin(
    returnUrl: string = environment.persistLoginEndpoint,
    internalUrl?: string
  ) {
    if (this.redirectInited) {
      return;
    }
    this.redirectInited = true;
    this.initRedirectionLogin(returnUrl, null, internalUrl);
  }

  private initRedirectionLogin(
    returnUrl: string,
    providerName?: string,
    internalUrl?: string
  ) {
    try {
      let securityKey = generateRandomString(15);
      // save original key
      sessionStorage.setItem(
        environment.redirectionLoginSecurityKeyFieldName,
        securityKey
      );
      // generate sha256
      let securityKeyHash = shajs('sha256').update(securityKey).digest('hex');

      let urlPrefix = providerName
        ? '/account/externallogin'
        : '/account/persistlogin';
      let url = `${
        this.baseUrl
      }${urlPrefix}?SecurityKeyHash=${encodeURIComponent(
        btoa(securityKeyHash)
      )}&returnUrl=${encodeURIComponent(returnUrl)}`;

      if (providerName) {
        url += `&Provider=${providerName}`;
      }
      if (internalUrl) {
        url += `&internalUrl=${encodeURIComponent(internalUrl)}`;
      }
      location.href = url;
    } catch (Exception) {
      this.redirectInited = false;
    }
  }

  claimAuthCode(authCode: string, purpose: string): Promise<boolean> {
    return new Promise((resolve) => {
      let securityKey = sessionStorage.getItem(
        environment.redirectionLoginSecurityKeyFieldName
      );
      if (!securityKey || !authCode) {
        resolve(false);
        return;
      }

      const body: claimAuthCodeDTO = {
        token: authCode,
        securityKey: btoa(securityKey),
        purpose: purpose,
      };
      this.http
        .post<TokenResponse>(`${this.baseUrl}/account/claimauthcode`, body)
        .subscribe(
          (resp) => {
            this.authenticateUser(resp);

            resolve(true);
          },
          () => resolve(false)
        );
    });
  }

  logout() {
    this.token = null;
    this.tokenExpiration = null;
    this._userHavePersistLogin = 0;

    this._userInfo = null;

    //eliminar persistent login
    this.http
      .get(`${this.baseUrl}/account/logout`, { withCredentials: true })
      .subscribe(
        () => {},
        () => {}
      );
  }

  tryRenewToken(): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      if (!this.isLoggedIn) {
        resolve(false);
        return;
      }
      let renovationTime = new Date();
      //Si el token le quedan menos de 35 minutos de vida
      renovationTime.setMinutes(renovationTime.getMinutes() + 35);
      if (renovationTime > this.tokenExpiration) {
        //No necesitamos renovar todavia
        resolve(false);
        return;
      }
      this.http
        .get<TokenResponse>(`${this.baseUrl}/account/refreshtoken`)
        .subscribe(
          (resp) => {
            this.authenticateUser(resp);
            resolve(true);
          },
          () => {
            this.logout();
            resolve(false);
          }
        );
    });
  }

  tryDecodeTokenClaims(): userInfo {
    const token = this.token;

    if (!token) {
      return null;
    }

    var tokenPayloadObject = JSON.parse(atob(token.split('.')[1]));

    return {
      userName: tokenPayloadObject[environment.claimTypes.username],
      userId: tokenPayloadObject[environment.claimTypes.userId],
      email: tokenPayloadObject[environment.claimTypes.email],
    };
  }

  getUserProfile(){

    if(!this.userInfo || !this.userInfo.userId){
      return of(null);
    }

    const url = `${this.baseUrl}/users/profile/${this.userInfo.userId}`;


      return this.http.get<UserProfile>(url);

  }

  getUserSocialNetworks(){
    if(!this.userInfo || !this.userInfo.userId){
      return of(null);
    }

    const url = `${this.baseUrl}/users/profile/${this.userInfo.userId}/socialnetworks`;

    return this.http.get<applicationUserSocalNetworks[]>(url);

  }
}
