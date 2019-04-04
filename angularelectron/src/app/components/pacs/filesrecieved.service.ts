import { Injectable } from '@angular/core';
import {ReceivedFile} from './filerecieved';
import * as Rx from "rxjs/Rx";


var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database('./pacs.db');

@Injectable()
export class FilesRecievedService {
    getFileNames():Rx.Observable<ReceivedFile[]>{
        return Rx.Observable.create(function(observer){
            let sql = `SELECT * FROM ReceivedFiles`;
            //debugger;
            db.all(sql, [], (err, rows) => {
                if (err) {
                    observer.error(new Error(err));                    
                }else{
                    observer.next(rows);     
                    observer.complete();                    
                }                
            });
        });        
    }
}