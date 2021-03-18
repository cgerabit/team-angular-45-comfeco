import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { promise } from 'selenium-webdriver';

@Injectable({
  providedIn: 'root'
})
export class LoadingOverlayService {

  constructor() { }

  private _loadingOverlayVisible:number=0;

  set loadingOverlayVisible(value:boolean){

    this._loadingOverlayVisible=value
    ?this._loadingOverlayVisible+1:this._loadingOverlayVisible-1;

    // Enviamos el callback al componente para actualizar el resultado
    this._overlayChanged.next(this._loadingOverlayVisible > 0);
  }

  get loadingOverlayVisible():boolean{

    // Si overlayvisible > 0 significa que todavia hay algo cargando
    return this._loadingOverlayVisible>0;

  }

  private _overlayChanged:Subject<boolean> =new Subject<boolean>();
  overlayChanged:Observable<boolean> = this._overlayChanged.asObservable();

  //Obtener un observale | promise y mostrar el loader hasta que termine
  setTimerWith<T>(func: Observable<T> | Promise<T>) :Promise<T>{

    return new Promise<T>((resolve,reject)=>{

      // Verificamos si es una promesa o un observable
      let isObs = func instanceof Observable
      let isPromise = func instanceof Promise
      if(isObs || isPromise){
        this.loadingOverlayVisible =true;
      }
  //definimos y comprobamos el tipo
      if(isObs){

        func = func as Observable<T>
        func.subscribe(r => {
          this.loadingOverlayVisible=false;
          resolve(r);

        },(err)=>{
          this.loadingOverlayVisible=false;
          reject(err);
        });
      }
      else if(isPromise){
          func = func as Promise<T>
          func.then(r => {
            resolve(r);
          })
          .catch((err)=>reject(err))
          .finally(()=>this.loadingOverlayVisible=false)
      }
      else{
        //func no valido retornamos null
        resolve(null);
      }

    });


  }
}

