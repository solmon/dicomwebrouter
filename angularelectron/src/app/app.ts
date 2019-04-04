import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
/*
 * Angular Modules
 */
import { enableProdMode, NgModule, Component } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { Ng2TableModule } from 'ng2-table/ng2-table';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { NAV_DROPDOWN_DIRECTIVES } from './components/shared/nav-dropdown.directive';
import { SIDEBAR_TOGGLE_DIRECTIVES } from './components/shared/sidebar.directive';
import { AsideToggleDirective } from './components/shared/aside.directive';
import { BreadcrumbsComponent } from './components/shared/breadcrumb.component';
//import { NgTableComponent, NgTableFilteringDirective, NgTablePagingDirective, NgTableSortingDirective } from 'ng2-table/ng2-table';

//Layouts
import { FullLayoutComponent } from './components/layouts/full-layout.component';
import { SimpleLayoutComponent } from './components/layouts/simple-layout.component';

import {DashboardModule} from './dashboard/dashboard.module';

// Setup redux with ngrx
import { Store, StoreModule } from '@ngrx/store';
import { reducers, initialState } from './store/index';

/**
 * Import our child components
 */
import { HomeComponent } from './components/home/home.component';
import { AppComponent } from './components/app.component';
import { PACSComponent } from './components/pacs/pacs.component';

/**
 * Import material UI Components
 */
import { MdButtonModule, MdSlideToggleModule } from '@angular/material';

import { routes } from './app.routes';

/**
 * Import the authentication service to be injected into our component
 */
import {FilesRecievedService} from './components/pacs/filesrecieved.service';

/*
 * provide('AppStore', { useValue: appStore }),
 */
@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        BrowserAnimationsModule,
        MdButtonModule,
        MdSlideToggleModule,
        Ng2TableModule,
        BsDropdownModule.forRoot(),
        TabsModule.forRoot(),
        ChartsModule,
        DashboardModule,
        RouterModule.forRoot(routes, { useHash: true }),
        StoreModule.forRoot(reducers, <any>initialState),
    ],
    providers: [FilesRecievedService],
    declarations: [AppComponent, 
                    HomeComponent, 
                    PACSComponent,
                    FullLayoutComponent,
                    SimpleLayoutComponent,
                    NAV_DROPDOWN_DIRECTIVES,
                    BreadcrumbsComponent,
                    SIDEBAR_TOGGLE_DIRECTIVES,
                    AsideToggleDirective],
    bootstrap: [AppComponent]
})
export class AppModule { }
platformBrowserDynamic().bootstrapModule(AppModule);
