import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { catchError, map, tap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthResponse, TokenResponse, Usuario } from '../interfaces/interfaces';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: String = environment.baseUrl;
  private _usuario: Usuario;

  get usuario() {
    return { ...this._usuario };
  }

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
        if (resp != null) {
          if (resp.token != '')
            localStorage.setItem('access_token', JSON.stringify(resp));
        }
      }),
      catchError((err: HttpErrorResponse) => of(err.error))
    );
  }


  validarToken(): Observable<boolean> {
    const url = `${this.baseUrl}/auth/renew`;
    const headers = new HttpHeaders().set(
      'x-token',
      localStorage.getItem('token') || ''
    );
    return this.http
      .get<AuthResponse>(url, { headers })
      .pipe(
        map((resp) => {
          //Establecer informacion del usuario
          localStorage.setItem('token', resp.token);

          this._usuario = {
            name: resp.name,
            uid: resp.uid,
            email: resp.email,
          };

          return resp.ok;
        }),
        catchError((err) => of(false))
      );
  }

  logout() {
    localStorage.removeItem("access_token");
  }
}
