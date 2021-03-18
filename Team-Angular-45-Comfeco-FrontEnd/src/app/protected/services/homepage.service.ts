import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { Comunity, Sponsor, ContentCreator,  Area, Country, Gender, SocialNetwork, GroupFilter, Group, ActiveEvent } from '../interfaces/interfaces';
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

    let queryParams = this.getPaginationParams(new HttpParams(),pagination);




    return this.http.get<Comunity[]>(url,{params:queryParams})
  }

  getSponsors(pagination:Pagination)
  {
    const url = `${this.baseUrl}/sponsors`;

       let queryParams = this.getPaginationParams(new HttpParams(),pagination);

       return this.http.get<Sponsor[]>(url,{params:queryParams});

  }

  getContentCreators(pagination:Pagination){

    const url = `${this.baseUrl}/users/contentcreators`

    let queryParams = this.getPaginationParams(new HttpParams(),pagination);



    return this.http.get<ContentCreator[]>(url,{params:queryParams})

  }

  getArea(id:number){
    const url =`${this.baseUrl}/areas/${id}`

    return this.http.get<Area>(url);

  }
  private getPaginationParams(params:HttpParams ,pagination:Pagination):HttpParams{

    return params.append('page',pagination.Page.toString())
    .append('RecordsPerPage',pagination.RecordsPerPage.toString());

  }


  updateSocialNetworks(){



  }

  getCountries(){
    const url =`${this.baseUrl}/countries`


    return this.http.get<Country[]>(url);

  }

  getGenders(){
    const url = `${this.baseUrl}/genders`;
    return this.http.get<Gender[]>(url);
  }

  getAreas(pagination:Pagination){

    const url = `${this.baseUrl}/areas`;

    var params = this.getPaginationParams(new HttpParams(),pagination);

    return this.http.get<Area[]>(url,{params});
  }

  getSocialNetworks(){
    const url = `${this.baseUrl}/socialnetworks`;

    return this.http.get<SocialNetwork[]>(url);

  }

  getGroups(groupFilter:GroupFilter){

    let params = new HttpParams();
    if(groupFilter.Name){
      params = params.append('Name',groupFilter.Name);
    }

    if(groupFilter.TechnologyId && groupFilter.TechnologyId>0){
      params = params.append("TechnologyId",groupFilter.TechnologyId.toString());

    }

    return this.http.get<Group[]>(`${this.baseUrl}/groups`,{params});
  }

  getEvents()
  {
    return this.http.get<ActiveEvent[]>(`${this.baseUrl}/events`);
  }

  getWorshops(){
    //return this.http.get
  }


}


