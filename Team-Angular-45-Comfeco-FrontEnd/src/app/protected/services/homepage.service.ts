import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { Comunity } from '../interfaces/interfaces';
import { Pagination } from '../../auth/interfaces/interfaces';

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

  getComunidades(pagination:Pagination){
    const url = `${this.baseUrl}/communities`

    let queryParams = new HttpParams()
    .append('page',pagination.Page.toString())
    .append('RecordsPerPage',pagination.RecordsPerPage.toString());



    return this.http.get<Comunity[]>(url,{params:queryParams})
  }
}


