import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MedicineComponent } from './medicine.component';
import { AddMedicineComponent } from '../add-medicine/add-medicine/add-medicine.component';
import { ConfirmLeaveService } from 'src/app/core/guards/confirm-leave.service';
import { MedicineDetailsComponent } from '../detail/medicine-details/medicine-details.component';

const routes: Routes = [
  {
    path:'',
    component: MedicineComponent,
  },
  {
    path:'add',
    component: AddMedicineComponent,
    canDeactivate: [ConfirmLeaveService]
  },
  {
    path:'details/:id',
    component: MedicineDetailsComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MedicineRoutingModule { }
