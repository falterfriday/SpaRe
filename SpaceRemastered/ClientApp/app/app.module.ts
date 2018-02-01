import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from "@angular/http";


import { AppComponent } from './app.component';
import { PhotoList } from "./photo/photoList.component";

import { DataService } from "./shared/dataService";

@NgModule({
  declarations: [
	  AppComponent,
	  PhotoList
  ],
  imports: [
	  BrowserModule,
	  HttpModule
  ],
  providers: [
	  DataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
