import { NgModule } from '@angular/core';
import { MWLModule } from '../mwl/mwl.module';
import { PTabsRoutingModule } from './home-routing.module';
import { PTabsComponent } from './home.component';
import { TabsModule } from 'ngx-bootstrap/tabs';

@NgModule({
  imports: [ TabsModule.forRoot(),PTabsRoutingModule,MWLModule],
  declarations: [
    PTabsComponent
  ],
  providers:[]
})
export class PTabsModule { }
