import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PTabsComponent } from './home.component';

const routes: Routes = [
  {
    path: '',
    component: PTabsComponent,
    data: {
      title: 'pacstabs'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PTabsRoutingModule {}
