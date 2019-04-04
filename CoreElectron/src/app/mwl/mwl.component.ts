import { Component,OnInit,NgZone } from '@angular/core';
import {MWLService} from './mwl.service';
import {Study,Series,Instance} from '../models';
import {SignalRService} from '../core/services/signalR.service';

@Component({
  selector: 'mwl',
  templateUrl: 'mwl.component.html'
})
export class MWLComponent implements OnInit {
  private mwlService:MWLService;
  private _signalRService: SignalRService;
  private studies:Study[];
  constructor(private mwl:MWLService,private signalRService: SignalRService,private _ngZone: NgZone) {     
    this.mwlService = mwl;
    this._signalRService = signalRService;
  } 
  ngOnInit(){
    this.mwlService.getStudies().subscribe(x=>{
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
