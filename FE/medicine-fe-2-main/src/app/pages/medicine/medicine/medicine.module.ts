import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MedicineRoutingModule } from './medicine-routing.module';
import { MedicineComponent } from './medicine.component';
import { AddMedicineComponent } from '../add-medicine/add-medicine/add-medicine.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CurrencyConversionPipe } from 'src/app/shared/pipes/currency-conversion.pipe';
import { SearchModule } from 'src/app/shared/modules/search/search.module';
import { SpinnerModule } from 'src/app/shared/modules/spinner/spinner.module';


@NgModule({
  declarations: [
    MedicineComponent,
    AddMedicineComponent,
    CurrencyConversionPipe,

  ],
  imports: [
    CommonModule,
    MedicineRoutingModule,

    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    FormsModule,
    SearchModule,
    SpinnerModule,
  

  ],
  providers: [CurrencyConversionPipe]
})
export class MedicineModule { }
