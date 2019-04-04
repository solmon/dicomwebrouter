import { Component,OnInit,NgZone,ChangeDetectorRef } from '@angular/core';
import {
    AEtitlesService ,DicomSendService ,LinkClientService ,RoutingTableService ,
    SettingsService } from '../services/masterservice';
import * as msModels from '../models';
import {SignalRService} from '../core/services/signalR.service';

import {Car} from '../models/car';
import {CarService} from '../services/carservice';
import { BaseComponent } from 'app/mastersettings/basecomponent';


@Component({
  selector: 'aeTitleSettings',
  templateUrl: 'aetitles.component.html'
})
export class AETitlesSettingComponent extends BaseComponent<msModels.AETitles> implements OnInit {
  
  constructor(private mwl:AEtitlesService,
    private signalRService: SignalRService,
    private ngZone: NgZone,
    private refh:ChangeDetectorRef) {     
    super(msModels.AETitles);
    
    this.mwlService = mwl;
    this.mwlService.setEntity('AETitle');
    this._signalRService = signalRService;
    this.ref = refh;
    this._ngZone = ngZone;    
  }
}

@Component({
  selector: 'dicomSendSettings',
  templateUrl: 'dicomsendsetting.component.html'
})
export class DicomSendSettingComponent extends BaseComponent<msModels.DicomSend> implements OnInit {
  
  constructor(private mwl:DicomSendService,
    private signalRService: SignalRService,
    private ngZone: NgZone,
    private refh:ChangeDetectorRef) {     
    super(msModels.DicomSend);
    
    this.mwlService = mwl;
    this.mwlService.setEntity('DicomSend');
    this._signalRService = signalRService;
    this.ref = refh;
    this._ngZone = ngZone;    
  }
}

@Component({
  selector: 'linkClientSettings',
  templateUrl: 'linkclients.component.html'
})
export class LinkClientSettingComponent extends BaseComponent<msModels.LinkClient> implements OnInit {
  
  constructor(private mwl:LinkClientService,
    private signalRService: SignalRService,
    private ngZone: NgZone,
    private refh:ChangeDetectorRef) {     
    super(msModels.LinkClient);
    
    this.mwlService = mwl;
    this.mwlService.setEntity('LinkClient');
    this._signalRService = signalRService;
    this.ref = refh;
    this._ngZone = ngZone;    
  }
}

@Component({
  selector: 'routetableSettings',
  templateUrl: 'routetables.component.html'
})
export class RouteTableSettingComponent extends BaseComponent<msModels.RoutingTable> implements OnInit {
  
  constructor(private mwl:RoutingTableService,
    private signalRService: SignalRService,
    private ngZone: NgZone,
    private refh:ChangeDetectorRef) {     
    super(msModels.RoutingTable);
    
    this.mwlService = mwl;
    this.mwlService.setEntity('RouteTable');
    this._signalRService = signalRService;
    this.ref = refh;
    this._ngZone = ngZone;    
  }
}

@Component({
  selector: 'basicSettings',
  templateUrl: 'basicsettings.component.html'
})
export class BasicSettingComponent extends BaseComponent<msModels.Settings> implements OnInit {
  
  constructor(private mwl:SettingsService,
    private signalRService: SignalRService,
    private ngZone: NgZone,
    private refh:ChangeDetectorRef) {     
    super(msModels.Settings);
    
    this.mwlService = mwl;
    this.mwlService.setEntity('Settings');
    this._signalRService = signalRService;
    this.ref = refh;
    this._ngZone = ngZone;    
  }
}

@Component({
  selector: 'baseSettings',
  templateUrl: 'mastersettings.component.html',
  styles: [`
  .ui-grid-row div {
    padding: 4px 10px
  }
  
  .ui-grid-row div label {
    font-weight: bold;
  }
`]
})
export class BaseSettingComponentCar implements OnInit {
  private mwlService:SettingsService;
  private _signalRService: SignalRService;
  private entites:msModels.Settings[];
  private _ngZone: NgZone

  constructor(private carService: CarService) { }
  /*
  constructor(private mwl:SettingsService,
    private signalRService: SignalRService,private _ngZone: NgZone
  ) {     
    this.mwlService = mwl;
    this._signalRService = signalRService;
  } */
  ngOnInit(){
    this.carService.getCarsSmall().subscribe(cars =>{ 
      this.cars = cars;
    });
    /*this.mwlService.getEntities().subscribe(x=>{
      this.entites = x;
    });
    this.subscribeToEvents();*/
  }
  private subscribeToEvents(): void {
    this._signalRService.incomingFiles.subscribe((rEnties: msModels.Settings[]) => {
        this._ngZone.run(() => {
            this.entites = rEnties
        });
    });
  }

  displayDialog: boolean;
  
  car: Car = new PrimeCar();
  
  selectedCar: Car;
  
  newCar: boolean;

  cars: Car[];      
    
  showDialogToAdd() {
      this.newCar = true;
      this.car = new PrimeCar();
      this.displayDialog = true;
  }
  
  save() {
      let cars = [...this.cars];
      if(this.newCar){
          cars.push(this.car);
      }
      else{
          cars[this.findSelectedCarIndex()] = this.car;
      }
      this.cars = cars;
      this.car = null;
      this.displayDialog = false;
  }
  
  delete() {
      let index = this.findSelectedCarIndex();
      this.cars = this.cars.filter((val,i) => i!=index);
      this.car = null;
      this.displayDialog = false;
  }    
  
  onRowSelect(event) {
      this.newCar = false;
      this.car = this.cloneCar(event.data);
      this.displayDialog = true;
  }
  
  cloneCar(c: Car): Car {
      let car = new PrimeCar();
      for(let prop in c) {
          car[prop] = c[prop];
      }
      return car;
  }
  
  findSelectedCarIndex(): number {
      return this.cars.indexOf(this.selectedCar);
  }
}
class PrimeCar implements Car {  
  constructor(public vin?, public year?, public brand?, public color?) {}
}
