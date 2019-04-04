import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Layouts
import { FullLayoutComponent } from './layouts/full-layout.component';
import { SimpleLayoutComponent } from './layouts/simple-layout.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'ptabs',
    pathMatch: 'full',
  },
  {
    path: '',
    component: FullLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: './dashboard/dashboard.module#DashboardModule'
      },
      {
        path: 'components',
        loadChildren: './components/components.module#ComponentsModule'
      },
      {
        path: 'widgets',
        loadChildren: './widgets/widgets.module#WidgetsModule'
      },
      {
        path: 'charts',
        loadChildren: './chartjs/chartjs.module#ChartJSModule'
      },
      {
        path: 'ptabs',
        loadChildren: './home/home.module#PTabsModule'
      },
      {
        path: 'monitor',
        loadChildren: './monitor/monitor.module#MonitorModule'
      },
      {
        path: 'masterdata',
        loadChildren: './mastersettings/mastersettings.module#MasterSettingsModule'
      }
    ]
  }
  // },
  // {
  //   path: 'pacsettings',
  //   component: FullLayoutComponent,
  //   data: {
  //     title: 'Pages'
  //   },
  //   children: [
  //     {
  //       path: '',
  //       loadChildren: './settings/pacsettings.module#PacSettingsModule',
  //     }
  //   ]
  // },
  // {
  //   path: 'mastersettings',
  //   component: FullLayoutComponent,
  //   data: {
  //     title: 'Pages'
  //   },
  //   children: [
  //     {
  //       path: 'set',
  //       loadChildren: './mastersettings/mastersettings.module#MasterSettingsModule',
  //     }
  //   ]
  // }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
