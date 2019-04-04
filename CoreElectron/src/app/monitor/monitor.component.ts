import { Component,OnInit,NgZone } from '@angular/core';
import {MonitorService} from './monitor.service';
import {Study,Series,Instance} from '../models';
import {SignalRService} from '../core/services/signalR.service';

@Component({
  selector: 'monitor',
  templateUrl: 'monitor.component.html'
})
export class MonitorComponent implements OnInit {
  private monitorService:MonitorService;
  private _signalRService: SignalRService;
  private studies:Study[];
  constructor(private monitors:MonitorService,private signalRService: SignalRService,private _ngZone: NgZone) {     
    this.monitorService = monitors;
    this._signalRService = signalRService;
  } 
  ngOnInit(){
    this.monitorService.getStudies().subscribe(x=>{
      this.studies = x;
    });
    this.subscribeToEvents();
  }
  private subscribeToEvents(): void {
    this._signalRService.incomingFiles.subscribe((inComingFiles: Study[]) => {
        this._ngZone.run(() => {
            this.studies = inComingFiles
        });
    });
  }
}
