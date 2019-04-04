import { MasterDataService } from "app/services/masterservice";
import { SignalRService } from "app/core/services/signalR.service";
import { ChangeDetectorRef } from "@angular/core/src/change_detection/change_detector_ref";
import { NgZone } from "@angular/core/src/zone/ng_zone";
import { NoParamConstructor, IdResolve } from "app/models";

export abstract class BaseComponent<TEntity extends IdResolve> {  
    protected mwlService:MasterDataService<TEntity> ;
    protected _signalRService: SignalRService;
    protected entites:TEntity[];
    protected ref:ChangeDetectorRef;
    protected _ngZone: NgZone;
    
    constructor(private ctor: NoParamConstructor<TEntity>) {
       
    }

    ngOnInit(){
      this.mwlService.getEntities().subscribe(x=>{
        this.entites = x;
        this.ref.detectChanges();
      });
      this.subscribeToEvents();
    }
    private subscribeToEvents(): void {
      this._signalRService.incomingFiles.subscribe((rEnties: TEntity[]) => {
          this._ngZone.run(() => {
              this.entites = rEnties
          });
      });
    }
  
    displayDialog: boolean;  
    entity: TEntity = new this.ctor();  
    selectedEntity: TEntity;
    
    newEntity: boolean;
    
    showDialogToAdd() {
        this.newEntity = true;
        this.entity = new this.ctor();
        this.displayDialog = true;
    }
    
    save() {
        let entities = [...this.entites];
        if(this.newEntity){
            this.mwlService.addEntity(this.entity).subscribe(x=>{
                entities.push(this.entity);
                this.ref.detectChanges();
            });            
        }
        else{
            entities[this.findSelectedCarIndex()] = this.entity;
            this.mwlService.updateEntity(this.entity).subscribe(x=>{
                //entities.push(this.entity);
                this.ref.detectChanges();
            });
        }
        this.entites = entities;
        this.entity = null;
        this.displayDialog = false;
    }
    
    delete() {
        let index = this.findSelectedCarIndex();
        this.entites = this.entites.filter((val,i) => i!=index);
        this.mwlService.deleteEntity(this.entity.GetId()).subscribe(x=>{
            //entities.push(this.entity);
            this.ref.detectChanges();
        });
        this.entity = null;
        this.displayDialog = false;
    }    
    
    onRowSelect(event) {
        this.newEntity = false;
        this.entity = this.cloneAETitle(event.data);
        this.displayDialog = true;
    }
    
    cloneAETitle(c: TEntity): TEntity {
        let entity = new this.ctor();
        for(let prop in c) {
          entity[prop] = c[prop];
        }
        return entity;
    }
    
    findSelectedCarIndex(): number {
        return this.entites.indexOf(this.selectedEntity);
    }  
  }