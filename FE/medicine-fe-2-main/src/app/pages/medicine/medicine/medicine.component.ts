import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Category } from 'src/app/core/models/Category';
import { Medicine } from 'src/app/core/models/Medicine';
import { MedicineService } from 'src/app/core/services/api/medicine.service';
import { CategoryService } from 'src/app/core/services/api/category.service';
import { forkJoin } from 'rxjs';
import { ConfirmComponent } from 'src/app/core/components/confirm/confirm.component';
import { CurrencyConversionPipe } from 'src/app/shared/pipes/currency-conversion.pipe';
import { ExportCsvService } from 'src/app/core/services/export-csv.service';

@Component({
  selector: 'app-medicine',
  templateUrl: './medicine.component.html',
  styleUrls: ['./medicine.component.css']
})
export class MedicineComponent {
  constructor(private medicineService: MedicineService,private categoryService: CategoryService,
    private router: Router, private dialog: MatDialog, private route: ActivatedRoute,
  private currencyPipe: CurrencyConversionPipe,
  private exportService: ExportCsvService) {

  }

  medicines: Medicine[] = [];

  categories: Category[]=[];

  selectedCategoryId: number = -1

  currentPage = 0;
  pageSize = 10;

  searchValue = "";

  errorMessage: string | null = null;

  isLoading: boolean = false;

  priceInUSD = false;

  ngOnInit(): void {
    this.isLoading = true;

    // this.getMedicines();
    // this.getCategories();

    forkJoin({
      medicines: this.medicineService.getMedicines(this.currentPage, this.pageSize, 0),
      categories: this.categoryService.fetchCategoryList()
    }).subscribe(
      (response) => {
        this.medicines = response.medicines;
        this.categories = response.categories;
        this.isLoading = false;
      },
      (error) => {
        window.alert('Error fetching medicines');
        console.error('Error fetching medicines:', error);
        this.isLoading = false;
    }
    )

    // this.searchService.getSearchValue().subscribe(value => {
    //   this.searchValue = value;
    //   this.doSearch(value);
    // });

  }


  getMedicines(): void {
    this.medicineService.getMedicines(this.currentPage, this.pageSize, 0).subscribe(
      (response) => {
        this.medicines = response;
        // console.log(this.medicines[0].images[0].name);
      },
      (error) => {
        window.alert('Error fetching medicines');
        console.error('Error fetching medicines:', error);
      }
    );
  }

  getCategories(): void {
    this.categoryService.fetchCategoryList().subscribe(
      (response) => {
        this.categories = response;

      },
      (error) => {
        console.error('Error fetching medicines:', error);
      }
    );

  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.getMedicines();
  }
  onSizeChange(event: Event): void {
    this.pageSize = Number((event.target as HTMLTextAreaElement).value);
    this.currentPage = 0;
    this.getMedicines();
  }

  onSeeDetailsMedicine(id: number){
    if(id !=null){
      this.router.navigate(['details', id], { relativeTo: this.route });
    }
  }


  onCategoryChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedCategoryId = Number(selectElement.value);
    this.fetchMedicinesByCategory(this.selectedCategoryId);
  }

  fetchMedicinesByCategory(categoryId: number) {
    if(categoryId > 0){
      this.medicineService.getMedicinesByCategory(categoryId).subscribe(
        (data: any[]) => {
          this.medicines = data;
          this.errorMessage = null;
        },
        (error: any) => {

          if (error.status === 404) {
            this.medicines = [];
            this.errorMessage = 'No medicines found for the selected category.';
          } else {
            this.medicines = []
            this.errorMessage = 'An error occurred while fetching medicines.';
          }

        }
      );
    }else{
      this.getMedicines();
    }
  }

  onDeleteMedicine(event: Event, medicine: Medicine){
    event.stopPropagation();

    const canDelete = medicine.id && medicine.id > 0;

    const dialogRef = this.dialog.open(ConfirmComponent, {
      width: '550px',
      data:{
        message: canDelete ?  "Do you want to process?" : "You cannot delete this product now.",
        canProcess: canDelete,
      }
    });
    dialogRef.afterClosed().subscribe(
      result =>{
        this.isLoading = true;
        if(result){
          this.medicineService.deleteMedicine(medicine.id).subscribe(
            res => {
              const indexToDelete = this.medicines.findIndex(m => m.id === medicine.id);
              if (indexToDelete !== -1) {
                  this.medicines.splice(indexToDelete, 1);
                  window.alert(`Delete medicine ${medicine.name} successfully`);
                  this.isLoading = false;
              } else {
                  console.error(`Medicine with id ${medicine.id} not found.`);
              }

            },
            err => {
              console.error(err);
              window.alert(`failed to Delete medicine ${medicine.name}`);
              this.isLoading = false;
            }
          )
        }else{
          this.isLoading = false;
        }
      }
    );
  }

  onSelectEdit(event: Event, medicine: Medicine){
    event.stopPropagation();
    if(medicine?.id !=null){
      this.router.navigate(['add'],  {
        relativeTo: this.route,
        queryParams: {edit:'true', id: medicine.id}
    });
    }
  }


  changeCurrency(){
    this.priceInUSD = !this.priceInUSD;
  }
  
  exportCSV(){
    const selectedFields = ['id','name','description','price','popularityMedicine']
    this.exportService.exportCSV(this.medicines, "medicines",selectedFields)
  }


}
