import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HandleApiErrorService } from '../handle-api-error.service';
import { Observable, catchError } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Constant } from '../../constant/Constant';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient,
              private handleApiErrorService: HandleApiErrorService
  ) { }

  delete(id: number): Observable<any>{
    return this.http.delete<any>(`${environment.apiUrl}/${Constant.API_ENDPOINT.IMAGE.BASE}/${id}`).pipe(
      catchError((error: HttpErrorResponse) => this.handleApiErrorService.handleError(error))
    )
  }
}
