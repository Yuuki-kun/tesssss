import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CheckDeactivate } from 'src/app/core/guards/Interface.Guard';
import { Category } from 'src/app/core/models/Category';
import { Medicine } from 'src/app/core/models/Medicine';
import { MedicineService } from 'src/app/core/services/api/medicine.service';
import { CategoryService } from 'src/app/core/services/api/category.service';
import { ImageService } from 'src/app/core/services/api/image.service';
import { AddCategoryComponent } from 'src/app/core/components/add-category/add-category.component';
import { ConfirmComponent } from 'src/app/core/components/confirm/confirm.component';

@Component({
  selector: 'app-add-medicine',
  templateUrl: './add-medicine.component.html',
  styleUrls: ['./add-medicine.component.css']
})
export class AddMedicineComponent implements OnInit, CheckDeactivate {
  medicineForm!: FormGroup;

  selectedFiles: File[] = [];

  medicineToSave: Medicine = {} as Medicine;
  categories: Category[] = [];
  selectedCategoryId: number = -1

  needConfirmLeave = true;

  isLoading: boolean = false;

  isEditing = false;

  constructor(private fb: FormBuilder, private categoryService: CategoryService,
              private medicineService: MedicineService,
              private imageService: ImageService,
              private dialog: MatDialog,
              private route: ActivatedRoute,
              private router: Router,
              private location: Location
            ) {
    this.route.queryParams.subscribe(params => {
      this.isEditing =(/true/i).test(params['edit'] ? "true" : "false");
      if(this.isEditing){
        const idParam = params['id'];
        console.log(this.isEditing, idParam);

        if (idParam !== null && idParam !== undefined) {
          this.isLoading = true;
          const id = +idParam;
          this.medicineService.getMedicineById(id).subscribe(
            {
              next: (res) => {
                this.medicineToSave = res;
              
                this.initForm(idParam);
              },
              error:(err) => {
                console.error(err);
                window.alert(err.error.title);
              },
              complete: () =>{
                this.isLoading = false;
              }
            }
          );
        } else {
          window.alert('The id is not found in the route parameter or is invalid, you will return to the home page');
          this.router.navigate(['/']);
        }
      }
    });

    this.initForm(-1);

  }



  checkDeactivate(currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot | undefined): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.needConfirmLeave ? confirm('Are you sure you want to leave this page?') : true;
  }

  private initForm(id: number){
    this.medicineForm = this.fb.group({
      id: [id || '-1'],
      name: [this.medicineToSave?.name  || '', {
        validators: [Validators.required, Validators.minLength(3), Validators.maxLength(255)],
        updateOn: 'change'
      }],
      description: [this.medicineToSave?.description || 'N/A',{
        updateOn: 'change'
      }],
      price: [this.medicineToSave?.price || '',{
        validators: [Validators.required, Validators.min(1), Validators.max(9999999)],
        updateOn: 'change'
      }],
      categoryId: [this.medicineToSave?.medicineCategory?.id || '0',
        {validators: [Validators.required, Validators.min(1), Validators.max(Number.MAX_VALUE)],
        updateOn: 'change'
    }],
      primaryImageId: ['0'],
      popularityMedicine: false
    }, {updateOn: 'change'});
  }


  ngOnInit(): void {
    this.categoryService.fetchCategoryList().subscribe(
      res => this.categories = res,
      err => console.error('Error fetching categories:', err)
    );
  }

  onCategoryChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedCategoryId = Number(selectElement.value);
  }

  onAddCategory(): void {
    const dialogRef = this.dialog.open(AddCategoryComponent, {
      width: '550px',
      data: {
        id: -1,
        name: '',
        title: 'Add new category'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.isLoading = true;
      if (result) {
        this.categoryService.addCategory(result).subscribe((res) => {
          console.log(res);

          this.categories.push(res);
          window.alert(`Category '${res}' added successfully.`)
          this.isLoading = false;
        },
        (err) => {
          window.alert(`Failed to add Category. ${err.message}.`);
          this.isLoading = false;

        }
      );
      }
    });
  }

  onFileSelected(event: any) {
    this.selectedFiles = event.target.files;
  }

  submitted = false;


  onSubmit() {
    this.submitted = true;

    if(this.medicineForm.invalid){
      window.alert("Invalid form");
      return;
    }

    this.isLoading = true;

    const formData: FormData = new FormData();

    formData.append('id', this.medicineForm.get('id')?.value);
    formData.append('name', this.medicineForm.get('name')?.value);
    formData.append('description', this.medicineForm.get('description')?.value || 'N/A');
    formData.append('price', this.medicineForm.get('price')?.value);
    formData.append('categoryId', this.medicineForm.get('categoryId')?.value);
    formData.append('popularityMedicine', this.medicineForm.get('popularityMedicine')?.value);

    for (let i = 0; i < this.selectedFiles.length; i++) {
      formData.append('images', this.selectedFiles[i], this.selectedFiles[i].name);
    }

    const formDataEntries = formData as any;
    for (const pair of formDataEntries.entries()) {
      console.log(`${pair[0]}, ${pair[1]}`);
    }

    this.medicineService.addMedicine(formData).subscribe(
      {
        next: (res) => {
          this.needConfirmLeave = false;
          console.log(res);
  
          if(this.isEditing){
            window.alert(`Update sccss.`);
            this.router.navigate(['/medicine']);
          }else{
            window.alert(`Add medicine ${res.name} Successfully.`);
            this.router.navigate(['/medicine']);
  
          }
  
          this.submitted = false;
        },
        error:(err) =>{
          window.alert(`Failed to add medicne. ${err.error.title}`);
          this.submitted = false;
  
        },
        complete: () =>{
          this.isLoading = false;
        }
      }
    )

  }

  onDeleteImage(id: number){
    const canDelete = id && id > 0;
    const dialogRef = this.dialog.open(ConfirmComponent, {
      width: '550px',
      data:{
        message: canDelete ?  "Do you want to process?" : "You cannot delete this image now.",
        canProcess: canDelete,
      }
    });


    dialogRef.afterClosed().subscribe(
      result =>{
        this.isLoading = true;
        if(result){

          this.isLoading = true;
          if(id != null && id != undefined){
            this.imageService.delete(id).subscribe(
              res=>{
              const idxtoRm = this.medicineToSave.images.findIndex(i => i.id = id);
              this.medicineToSave.images.splice(idxtoRm -1, 1);
              this.isLoading = false;
            },
          err=>{
            console.error(err);
            window.alert(`Failed to remove image. ${err.error.title}`);
            this.isLoading = false;
          })
          }
        }else{
          this.isLoading = false;
        }
      }
    );

  }

  getField(name: string){
    return this.medicineForm.get(name);
  }

  goBack(){
    this.location.back();
  }

  goToViewProd(){
    this.router.navigate(["/medicine/details", this.medicineToSave.id]);
  }

  onResetFormData(){
    this.initForm(-1);
    this.selectedFiles = [];
  }

}
