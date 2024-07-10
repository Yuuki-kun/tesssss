import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AddCategoryComponent } from 'src/app/core/components/add-category/add-category.component';
import { ConfirmComponent } from 'src/app/core/components/confirm/confirm.component';
import { Category } from 'src/app/core/models/Category';
import { CategoryService } from 'src/app/core/services/api/category.service';
import { ExportCsvService } from 'src/app/core/services/export-csv.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent {
  constructor(private categoryService: CategoryService,
    private exportService: ExportCsvService
    , private router: Router,private dialog: MatDialog, private route: ActivatedRoute){}

  categories: Category[] = [];
  selectedCategory!: Category;
  isEditing = false;
  isSeeDetails: boolean = false;
  isLoading = false;
  ngOnInit(): void {
    this.isLoading = true;
    this.categoryService.fetchCategoryList().subscribe(
      {
        next: res => this.categories = res,
        error: err => {
          window.alert(err.error.title);
          console.error(err);
        },
        complete: () => this.isLoading = false
      }
     
    )
  }
  onSeeDetailsCategory(category: Category){
    if(category !=null){
      this.isSeeDetails = true;
      this.selectedCategory = category;
      this.router.navigate(["details",this.selectedCategory.id], {relativeTo: this.route});
    }
  }

  onCloseSeeDetailsCategory(){
    this.isSeeDetails = false;
  }

  onAddCategory(): void {
    const dialogRef = this.dialog.open(AddCategoryComponent, {
      width: '550px',
      data:{
        id: this.isEditing ? this.selectedCategory.id : -1,
        title: this.isEditing ? "Edit Category" : "Add new category",
        name: this.isEditing ? this.selectedCategory.name : '',
        editing: this.isEditing
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);

          if(!this.isEditing)  this.categories.push(result);

          else{

            const temp = this.categories.map(obj => {
              if (obj.id === this.selectedCategory.id) {
                  return { ...obj, name: result };
              }
              return obj;
          });
            this.categories = temp;
            console.log(this.categories);
            this.isEditing =false;

          }
        }
    });
  }

  onDelete(e: Event, category: Category){

    e.stopPropagation();

    const canDelete = category.medicines?.length === 0
                      && category.id !==null && category.id !== undefined && category.id > 0;

    const dialogRef = this.dialog.open(ConfirmComponent, {
      width: '550px',
      data:{
        message: canDelete ?  "Do you want to process?" : "You cannot delete this category now.",
        canProcess: canDelete
      }
    });

    dialogRef.afterClosed().subscribe(
      result => {
        console.log(result);
        if(result){
          //delete category
          this.categoryService.deleteCategory(category.id).subscribe(
            res => {
              const indexToDelete = this.categories.findIndex(m => m.id === category.id);
              if (indexToDelete !== -1) {
                  this.categories.splice(indexToDelete, 1);
                  window.alert(`Delete category ${category.name} successfully`);
              } else {
                  console.error(`Category with id ${category.id} not found.`);
              }

            },
            err => {
              console.error(err);
              window.alert(`failed to Delete category ${category.name}`);
            }
          )
        }else{
        }
      }
    )
  }

  onSelectEdit(e: Event, category: Category){
    e.stopPropagation();
    this.isEditing = true;
    this.selectedCategory = category;
    this.onAddCategory();
  }

  exportCSV(){
    const selectedFields = ['id','name'];
    this.exportService.exportCSV(this.categories, "categories",selectedFields);
  }

}
