import { Injectable } from '@angular/core';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class LocalService {

  constructor(private storageService: StorageService) { }
  // Set the json data to local
  setJsonValue(key: string, value: any) {
    this.storageService.secureStorage.setItem(key, value);
  }
  // Get the json value from local
  getJsonValue(key: string) {
    return this.storageService.secureStorage.getItem(key);
  }// Clear the local
  clearToken(key: string) {
    return this.storageService.secureStorage.removeItem(key);
  }
}

