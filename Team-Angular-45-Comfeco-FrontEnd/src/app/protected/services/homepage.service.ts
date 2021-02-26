import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomepageService {

  private baseUrl: String = environment.baseUrl;
  private _evento: Event;

  constructor(private http: HttpClient) {

   }

   get event() {
    return { ...this._evento };
  }

  eventInfo() {
    const url = `${this.baseUrl}/events`;
    return this.http.get(url).pipe(
      tap((resp) => {
        console.log(resp);
      }),
      catchError((err) => of(err.error))
    );
  }

  getTecnologias() {
    const url = `${this.baseUrl}/technologies`;
    return this.http.get(url).pipe(
      tap((resp) => {
        console.log(resp);
      }),
      catchError((err) => of(err.error))
    );
  }
}


