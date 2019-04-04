import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpModule} from '@angular/http';
import { FormsModule } from '@angular/forms';

import {DataTableModule, InputTextareaModule, 
        PanelModule, DropdownModule,
       DialogModule,ConfirmDialogModule,CheckboxModule} from 'primeng/primeng';
import {  AETitlesSettingComponent,
          BasicSettingComponent,
          DicomSendSettingComponent,
          LinkClientSettingComponent,
          RouteTableSettingComponent 
        } from './mastersettings.component';
import { MasterSettingsRoutingModule } from './mastersettings-routing.module';
import { 
        AEtitlesService , DicomSendService ,LinkClientService ,RoutingTableService ,
        SettingsService } from '../services/masterservice';
import {AppConfig} from '../app.config';
import * as msModels from '../models';

import {CarService} from '../services/carservice';

@NgModule({
  imports: [ CommonModule,HttpModule,MasterSettingsRoutingModule,FormsModule,
    DataTableModule, InputTextareaModule, PanelModule, DropdownModule,DialogModule,ConfirmDialogModule,CheckboxModule
  ],
  declarations: [
    AETitlesSettingComponent,
    BasicSettingComponent,    
    DicomSendSettingComponent,
    LinkClientSettingComponent,
    RouteTableSettingComponent
  ],
  exports: [AETitlesSettingComponent,
            BasicSettingComponent,
            DicomSendSettingComponent,
            LinkClientSettingComponent,
            RouteTableSettingComponent
          ],
  providers:[
      AEtitlesService, DicomSendService ,LinkClientService ,RoutingTableService ,
      SettingsService
    ,AppConfig,CarService]
})
export class MasterSettingsModule { }

