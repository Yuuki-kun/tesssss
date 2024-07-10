import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CategoryService } from '../../services/api/category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent {
  categoryForm: FormGroup;

  constructor(private categoryService: CategoryService, private fb: FormBuilder, private dialogRef: MatDialogRef<AddCategoryComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
    this.categoryForm = this.fb.group({
      id: [data?.id],
      name: [data?.name, [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255),
        Validators.pattern('^[a-zA-Z0-9 ]*$')
      ]]    });
  }
  ngOnInit(): void { }

  onSubmit(): void {
    if (this.categoryForm.valid) {

      if(this.categoryForm.controls['name'].value === this.data.name){
        window.alert('Nothing change.')
        this.dialogRef.close();
        return;
      }

      this.categoryService.addCategory(this.categoryForm.value).subscribe((res) => {
        console.log(res);
        if(this.data?.id == null || this.data?.id == undefined || this.data?.id <= 0){
          window.alert(`Add Category '${res.name}' successfully.`);
          this.dialogRef.close(res);
        }
        else{
          window.alert(`Updated`);
          this.dialogRef.close(this.categoryForm.controls['name'].value);
        }
      },
      (err) => {

        window.alert(`Failed to add/edit Category.`);
      }
    );
    }else{
      window.alert("Form invalid");
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
