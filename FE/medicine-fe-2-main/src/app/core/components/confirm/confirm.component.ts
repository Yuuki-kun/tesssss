import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent {
  confirm: boolean = false;

  constructor(private dialogRef: MatDialogRef<ConfirmComponent>, @Inject(MAT_DIALOG_DATA) public data: any){

  }

  ngOnInit(){

  }

  onSubmit(): void {
      this.confirm = true;
      this.dialogRef.close(this.confirm);
  }
  onCancel(): void {
    this.confirm = false;
    this.dialogRef.close(this.confirm);
  }
}
