import { Component } from '@angular/core';
import { MedicineService } from './core/services/api/medicine.service';
import { Medicine } from './core/models/Medicine';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = 'MedicineApp';
 
}
