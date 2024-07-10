import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './pages/layouts/main-layout/main-layout.component';

const routes: Routes = [
  {
      path: '',
      component: MainLayoutComponent,
      children: [
        {
          path: 'medicine',
          loadChildren: () => import('./pages/medicine/medicine/medicine.module').then(m => m.MedicineModule)
        },
        {
          path: 'category',
          loadChildren: () => import('./pages/category/category/category.module').then(m => m.CategoryModule)
        }
      ],
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
