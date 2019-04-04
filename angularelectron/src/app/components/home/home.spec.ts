import {
    async,
    inject,
    TestBed,
} from '@angular/core/testing';
import { Component } from '@angular/core';
import { HomeComponent } from './home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Setup redux with ngrx
import { Store, StoreModule } from '@ngrx/store';
import { reducers, initialState } from './../../store/index';

describe('App component', () => {
    beforeEach(() => TestBed.configureTestingModule({
        imports: [
            FormsModule,
            ReactiveFormsModule,
            StoreModule.forRoot(reducers, <any>initialState),
        ],
        providers: [
            HomeComponent,
        ],
    }));

    it('should have default data', inject([HomeComponent], (home: HomeComponent) => {
        expect(home.messageForm.controls['messageText'].value).toEqual('Angular2');
    }));
});
