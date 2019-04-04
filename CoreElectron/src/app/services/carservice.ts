import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Observable';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Car } from '../models/car';

@Injectable()
export class CarService {

    constructor(private http: Http) { }

    getCarsSmall(): Observable<Car[]> {
    return this.http.get('assets/showcase/data/cars-small.json')
    .map(res => {    
        return res.json().data || []});
    }

    getCarsMedium(): Observable<Car[]> {
    return this.http.get('assets/showcase/data/cars-medium.json')
    .map(res => {    
        return res.json().data || []});
    }

    getCarsLarge(): Observable<Car[]> {
    return this.http.get('assets/showcase/data/cars-medium.json')
    .map(res => {    
        return res.json().data || []});
    }
}