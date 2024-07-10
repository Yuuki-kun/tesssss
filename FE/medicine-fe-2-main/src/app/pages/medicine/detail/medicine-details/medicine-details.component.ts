import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Medicine } from 'src/app/core/models/Medicine';
import { MedicineService } from 'src/app/core/services/api/medicine.service';

@Component({
  selector: 'app-medicine-details',
  templateUrl: './medicine-details.component.html',
  styleUrls: ['./medicine-details.component.css']
})
export class MedicineDetailsComponent {
  medicine!: Medicine;

  constructor(
    private medicineService: MedicineService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam !== null && idParam !== undefined) {
        const id = +idParam;
        this.medicineService.getMedicineById(id).subscribe(
          (res) => {
            this.medicine = res;
          },
          (err) => {
            console.error(err);
            window.alert(err.error.title);
          }
        );
      } else {
        window.alert('The id is not found in the route parameter or is invalid, you will return to the home page');
        this.router.navigate(['/']);
      }
    });
  }

  onSelectEdit(medicine: Medicine){
    if(medicine?.id !=null){
      this.router.navigate(['/medicine/add'],  {
        relativeTo: this.route,
        queryParams: {edit:'true', id: medicine.id}
    });
    }
  }

  back(){
    this.location.back();
  }
}
