import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse
} from '@angular/common/http';

import { catchError, tap} from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import * as shajs from 'sha.js';
import { environment } from '../../../environments/environment';
import { TokenResponse, Usuario, UserGroup, changeUsernameDTO, changeEmailDTO, changePasswordDTO, UserProviders } from '../interfaces/interfaces';
import { claimAuthCodeDTO } from '../DTOs/claimAuthCodeDTO';

import {
  generateRandomString,
  setOrDeleteFromStorage,
} from '../../../utils/Utilities';
import { userInfo } from '../interfaces/userInfo';




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




  //---------------------------------------- END GETTERS ----------------------------

  constructor(private http: HttpClient) {}

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
    else{
      this.tryRenewToken(true);
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

  tryRenewToken(overrideTimer:boolean=false): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      if (!this.isLoggedIn) {
        resolve(false);
        return;
      }
      let renovationTime = new Date();
      //Si el token le quedan menos de 35 minutos de vida
      renovationTime.setMinutes(renovationTime.getMinutes() + 35);
      if ( !overrideTimer && renovationTime > this.tokenExpiration) {
        //No necesitamos renovar todavia
        return resolve(false);
      }
      this.http
        .get<TokenResponse>(`${this.baseUrl}/account/refreshtoken`)
        .subscribe(
          (resp) => {
            this.userHavePersistLogin.then(r => {
              resp.isPersistent = r;
              this.authenticateUser(resp);
              resolve(true);
            })
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

  changeUsername(changeUsernameDTO:changeUsernameDTO){

    if(!changeUsernameDTO.userId){
      changeUsernameDTO.userId = this.userInfo.userId
    }

    return this.http.post(`${this.baseUrl}/account/changeusername`,changeUsernameDTO);

  }

  changeEmail(changeEmail:changeEmailDTO){
      if(!changeEmail.userId){
        changeEmail.userId = this.userInfo.userId;
      }

      return this.http.post(`${this.baseUrl}/account/changeemail`,changeEmail);


  }

  changePassword(changePasswordDTO:changePasswordDTO){
    if(!changePasswordDTO.userId){
      changePasswordDTO.userId = this.userInfo.userId
    }

    return this.http.post(`${this.baseUrl}/account/changepwd`,changePasswordDTO)

  }


  initExternalProviderLink(providerName:string){
    let url = `${this.baseUrl}/account/initexternalloginlink?token=${this.token}&ProviderName=${providerName}`;

    location.href=url;
  }

  userProviders():Observable<UserProviders>{

    return this.http.get<UserProviders>(`${this.baseUrl}/account/getUserLogins/${this.userInfo.userId}`)
  }
}
