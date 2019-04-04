import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AETitlesSettingComponent,
         BasicSettingComponent,
         LinkClientSettingComponent,
         DicomSendSettingComponent,
         RouteTableSettingComponent
 } from './mastersettings.component';

const routes: Routes = [
  {
    path: 'settings',
    component: BasicSettingComponent,
    data: {
      title: 'settings'
    }    
  },
  {
    path: 'aetitles',
    component: AETitlesSettingComponent,
    data: {
      title: 'AE Titles Management'
    }    
  },
  {
    path: 'dicomsend',
    component: DicomSendSettingComponent,
    data: {
      title: 'Dicom Send Management'
    }    
  },
  {
    path: 'linkclients',
    component: LinkClientSettingComponent,
    data: {
      title: 'Link Clients Management'
    }    
  },
  {
    path: 'routetableSettings',
    component: RouteTableSettingComponent,
    data: {
      title: 'Routing Management'
    }    
  }   
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MasterSettingsRoutingModule {}
