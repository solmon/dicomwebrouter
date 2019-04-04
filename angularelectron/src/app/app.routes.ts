import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { PACSComponent } from './components/pacs/pacs.component';
import { FullLayoutComponent } from './components/layouts/full-layout.component';
import { SimpleLayoutComponent } from './components/layouts/simple-layout.component';
import {DashboardModule} from './dashboard/dashboard.module';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
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
        component: DashboardModule
        //loadChildren: './dashboard/dashboard.module#DashboardModule'
      }/*,
      {
        path: 'components',
        component: 
        loadChildren: './components/components.module#ComponentsModule'
      },
      {
        path: 'icons',
        loadChildren: './icons/icons.module#IconsModule'
      },
      {
        path: 'widgets',
        loadChildren: './widgets/widgets.module#WidgetsModule'
      },
      {
        path: 'charts',
        loadChildren: './chartjs/chartjs.module#ChartJSModule'
      }*/
    ]
  },
  {
    path: 'pages',
    component: SimpleLayoutComponent,
    data: {
      title: 'Pages'
    },
    children: [
      {
        path: '',
        loadChildren: './pages/pages.module#PagesModule',
      }
    ]
  }
];
