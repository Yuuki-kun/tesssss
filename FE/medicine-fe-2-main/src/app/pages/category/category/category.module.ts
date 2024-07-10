import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CategoryRoutingModule } from './category-routing.module';
import { CategoryComponent } from './category.component';
import { SearchModule } from 'src/app/shared/modules/search/search.module';
import { NaPipe } from 'src/app/shared/pipes/na.pipe';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { SpinnerModule } from "../../../shared/modules/spinner/spinner.module";


@NgModule({
  declarations: [
    CategoryComponent,
    NaPipe
  ],
  imports: [
    CommonModule,
    CategoryRoutingModule,
    SearchModule,
    SpinnerModule
],
  providers: [NaPipe, { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } },
   { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: false } }

  ]
})
export class CategoryModule { }
