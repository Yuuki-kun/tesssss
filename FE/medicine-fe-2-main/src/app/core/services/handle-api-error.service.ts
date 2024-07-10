import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HandleApiErrorService {

  constructor() { }

  handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Unknown error occurred.';
    if(error.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${error.error.message}`;
    }else{
      errorMessage = `Server returned code ${error.status}, error message is: ${error.message}`;
    }
    console.error(errorMessage);
    
    return throwError(()=>errorMessage);
  }
}
