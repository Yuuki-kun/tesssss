import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { MainLayoutComponent } from './pages/layouts/main-layout/main-layout.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import {  MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddCategoryComponent } from './core/components/add-category/add-category.component';
import { ConfirmComponent } from './core/components/confirm/confirm.component';
import { MedicineDetailsComponent } from './pages/medicine/detail/medicine-details/medicine-details.component';

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    AddCategoryComponent,
    ConfirmComponent,
    MedicineDetailsComponent,
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
