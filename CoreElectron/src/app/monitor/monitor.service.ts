import 'rxjs/add/operator/map';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Study } from '../models';
import { AppConfig } from '../app.config';

@Injectable()
export class MonitorService {
  constructor(private http: Http, private appConfig: AppConfig) { }
  getStudies(): Observable<Study[]> {
    return this.http.get(`${this.appConfig.ApiUrl}monitor/servers`)
      .map(res => {    
        return res.json() || []});
  }
}
