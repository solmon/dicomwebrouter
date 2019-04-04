import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpModule} from '@angular/http';
import { MonitorComponent } from './monitor.component';
import { MonitorService } from './monitor.service';
import {AppConfig} from '../app.config';
import { MonitorRoutingModule } from './monitor-routing.module';

@NgModule({
  imports: [ CommonModule,HttpModule,MonitorRoutingModule],
  declarations: [
    MonitorComponent
  ],
  exports: [MonitorComponent],
  providers:[MonitorService,AppConfig]
})
export class MonitorModule { }

