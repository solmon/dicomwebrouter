import 'rxjs/add/operator/map';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import  * as Models from '../models';
import { AppConfig } from '../app.config';
import * as msModels from '../models';
//@Injectable()

export class MasterDataService<Entity> {
  protected entity:string;
  constructor(public http: Http, public appConfig: AppConfig) { }
  setEntity(entity:string){
    this.entity = entity;
  }
  getEntities(): Observable<Entity[]> {
    return this.http.get(`${this.appConfig.ApiUrl}` + this.entity + `/all`)
      .map(res => {    
        return res.json() || []});
  }
  getEntity(id:string): Observable<Entity> {
    return this.http.get(`${this.appConfig.ApiUrl}` + this.entity + `/byid` + id)
      .map(res => {    
        return res.json() || []});
  }
  addEntity(aeTitle:Entity): Observable<boolean> {
    return this.http.post(`${this.appConfig.ApiUrl}` + this.entity + `/add`,aeTitle)
      .map(res => {    
        return res.json() || []});
  }
  updateEntity(aeTitle:Entity): Observable<boolean> {
    return this.http.post(`${this.appConfig.ApiUrl}` + this.entity + `/update`,aeTitle)
      .map(res => {    
        return res.json() || []});
  }
  deleteEntity(eId:string): Observable<boolean> {
    return this.http.delete(`${this.appConfig.ApiUrl}` + this.entity + `/` + eId)
      .map(res => {    
        return res.json() || []});
  }
}

@Injectable()
export class AEtitlesService extends MasterDataService<msModels.AETitles> {
  constructor(public http: Http, public appConfig: AppConfig) {
    super(http,appConfig);
  }
}

@Injectable()
export class DicomSendService extends MasterDataService<msModels.DicomSend> {
  constructor(public http: Http, public appConfig: AppConfig) {
    super(http,appConfig);
  }
}

@Injectable()
export class LinkClientService extends MasterDataService<msModels.LinkClient> {
  constructor(public http: Http, public appConfig: AppConfig) {
    super(http,appConfig);
  }
}

@Injectable()
export class RoutingTableService extends MasterDataService<msModels.RoutingTable> {
  constructor(public http: Http, public appConfig: AppConfig) {
    super(http,appConfig);
  }
}

@Injectable()
export class SettingsService extends MasterDataService<msModels.Settings> 
{
  constructor(public http: Http, public appConfig: AppConfig) {
    super(http,appConfig);
  }
}