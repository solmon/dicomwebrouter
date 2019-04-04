import { NgModule } from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpModule} from '@angular/http';
import { MWLComponent } from './mwl.component';
import { MWLService } from './mwl.service';
import {AppConfig} from '../app.config';

@NgModule({
  imports: [ CommonModule,HttpModule],
  declarations: [
    MWLComponent
  ],
  exports: [MWLComponent],
  providers:[MWLService,AppConfig]
})
export class MWLModule { }

