import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { catchError, map, tap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthResponse, Usuario } from '../interfaces/interfaces';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl: String = environment.baseUrl;
  private _usuario: Usuario;

  get usuario(){
    return { ...this._usuario};
  }

  constructor( private http: HttpClient) { }


  register(name: string, email: string, password:string){
    const url = `${this.baseUrl}/api/account/register`;
    const body = {name:name, email: email, password:password};

    return  this.http.post<AuthResponse>(url, body)
      .pipe(
        tap( resp =>{
          if(resp.ok){

            localStorage.setItem('access_token', resp.token);
          }

        }),
        map(resp => resp.ok ),
        catchError(err => of(err.error.msg) )
      );


  }

  login(email: string, password:string){

    const url = `${this.baseUrl}/api/account/login`;
    const body = { email: email, password:password};

    return  this.http.post<AuthResponse>(url, body)
      .pipe(
        tap( resp =>{
          if(resp.ok){

            localStorage.setItem('access_token', resp.token);
          }

        }),
        map(resp => resp.ok ),
        catchError(err => of(err.error.msg) )
      );
  }

  validarToken():Observable<boolean>{
    const url = `${this.baseUrl}/api/account/renew`;
    const headers = new HttpHeaders()
      .set('Authorization', localStorage.getItem('token') || '');

    return this.http.get<AuthResponse>( url, { headers} )
      .pipe(
        map( resp => {
          //Establecer informacion del usuario
          localStorage.setItem('access_token', resp.token);

          this._usuario = {
            name: resp.name,
            uid:resp.uid,
            email: resp.email,
          }

          return resp.ok;
        }),
        catchError(err => of(false))
      );
  }

  logout(){
    localStorage.clear();
  }


}
