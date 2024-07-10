import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Category } from 'src/app/core/models/Category';
import { Medicine } from 'src/app/core/models/Medicine';
import { CategoryService } from 'src/app/core/services/api/category.service';
import { MedicineService } from 'src/app/core/services/api/medicine.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchResultComponent {
  @Input() type: 'medicine' | 'category' = 'medicine';

  initialValue = "";
  results:any;
  searchRequestSubscriptions: Subscription[] = [];

  isSearching: boolean = false;

  constructor(private medicineService: MedicineService,
    private categoryService: CategoryService,
    private router: Router,
    private route: ActivatedRoute
  ){

  }

  onTextChange(changedText: string){
    this.cancelPendingRequests();
    this.isSearching = true;
    if(changedText.length==0){
      this.results = [];
      this.isSearching = false;
    }else{
      const medicineSub = this.type === 'medicine' ? this.medicineService.searchMedicines(changedText).subscribe(
        res => this.results = res,
        err => {
          alert("search err");
          console.error(err);
          this.isSearching = false;
        }
      ) :
      this.categoryService.searchCategory(changedText).subscribe(
        res => this.results = res,
        err => {
          alert("search err");
          console.error(err);
          this.isSearching = false;
        }
      )
      ;
      this.searchRequestSubscriptions.push(medicineSub);
    }

  }
  cancelPendingRequests() {
    this.searchRequestSubscriptions.forEach(sub => sub.unsubscribe());
  }

  onLinkClick(id: any) {
      this.initialValue = '';
      if(this.type === "medicine"){
        this.router.navigate(["details/", id], {relativeTo: this.route});

    }

  }
}
