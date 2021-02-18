import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';

import { catchError, map, tap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import * as shajs from 'sha.js';
import { environment } from '../../../environments/environment';
import { TokenResponse, Usuario } from '../interfaces/interfaces';
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
  private _token: string = '';

  get token(): string {
    if (!this._token) {
      this._token = localStorage.getItem(environment.tokenFieldName);
    }
    return this._token;
  }

  set token(value: string) {
    localStorage.setItem(environment.tokenFieldName, value);
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
    localStorage.setItem(
      environment.tokenExpirationFieldName,
      value.toString()
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
  private _userHavePersistLogin: number;
  get userHavePersistLogin(): Promise<boolean>
  {
    return new Promise<boolean>((resolve) => {
      if (this._userHavePersistLogin) {
        let result = this._userHavePersistLogin === 1;
        resolve(result);
        return;
      }
      const persistentStorageValue = localStorage.getItem(
        environment.isPersistentKeyFieldName
      );
      if (persistentStorageValue) {
        const persistentValue = parseInt(persistentStorageValue);
        if (persistentValue === 1) {
          this.http.get(`${this.baseUrl}/account/havepersistlogin`,{withCredentials:true}).subscribe(
            () => {
              this._userHavePersistLogin = 1;
              localStorage.setItem(environment.isPersistentKeyFieldName,"1");
              resolve(true);
            },
            () => {
              this._userHavePersistLogin = 0;
              localStorage.setItem(environment.isPersistentKeyFieldName,"0");
              resolve(false);
            }
          );
        } else {
          resolve(false);
        }
      } else {
        resolve(false);
      }
    });
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

    return this.http.post<TokenResponse>(url, body,{withCredentials:true}).pipe(
      tap((resp) => {
        this.authenticateUser(resp);
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
      let persistentStorageValue = tokenResp.isPersistent?1:0;
      localStorage.setItem(environment.isPersistentKeyFieldName,persistentStorageValue.toString());

    }
  }

  initLoginWithExternalProvider(
    providerName: string,
    returnUrl: string = environment.externalLoginEndpoint
  ) {
    this.initRedirectionLogin(returnUrl, providerName);
  }
  initPersistentLogin(returnUrl: string = environment.persistLoginEndpoint,internalUrl?:string) {
    this.initRedirectionLogin(returnUrl,null,internalUrl);
  }

 private initRedirectionLogin(returnUrl: string, providerName?: string,internalUrl?:string) {
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
    let url = `${this.baseUrl}${urlPrefix}?SecurityKeyHash=${encodeURIComponent(
      btoa(securityKeyHash)
    )}&returnUrl=${encodeURIComponent(returnUrl)}`;

    if (providerName) {
      url += `&Provider=${providerName}`;
    }
    if(internalUrl){
      url+=`&internalUrl=${encodeURIComponent(internalUrl)}`
    }

    location.href = url;
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

    localStorage.removeItem(environment.tokenFieldName);
    localStorage.removeItem(environment.tokenExpirationFieldName);
    localStorage.removeItem(environment.isPersistentKeyFieldName);

    //eliminar persistent login
    this.http.get(`${this.baseUrl}/account/logout`,{withCredentials:true}).subscribe(()=>{},()=>{});
  }
<<<<<<< Updated upstream
=======

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
>>>>>>> Stashed changes
}
