import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
  HttpResponse,
} from '@angular/common/http';
import { Observable, catchError, delay, pipe } from 'rxjs';
import { Medicine } from '../../models/Medicine';
import { environment } from 'src/environments/environment.development';
import { Constant } from '../../constant/Constant';
import { HandleApiErrorService } from '../handle-api-error.service';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root',
})
export class MedicineService {
  constructor(
    private http: HttpClient,
    private handleApiErrorService: HandleApiErrorService,
    private router: Router
  ) {}

  getMedicines(
    page: number = 0,
    size: number = 0,
    sort: number = 0
  ): Observable<Medicine[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString())
      .set('d', sort.toString());

    return this.http
      .get<Medicine[]>(
        `${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.GET_LIST}`,
        { params: params }
      )
      .pipe(
        catchError((error: HttpErrorResponse) =>
          this.handleApiErrorService.handleError(error)
        )
      );
  }

  searchMedicines(searchValue: string = ''): Observable<Medicine[]> {
    return this.http
      .get<Medicine[]>(
        `${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.SEARCH}`,
        {
          params: {
            name: searchValue,
          },
        }
      )
      .pipe(
        catchError((error: HttpErrorResponse) =>
          this.handleApiErrorService.handleError(error)
        )
      );
  }

  getMedicineById(id: number): Observable<Medicine> {

    if (id === undefined || isNaN(id) || id <= 0) {
      window.alert(
        "The id is not found in the route parameter or is invalid, you will return to the home page",
      );
      this.router.navigate(["/"]);
      throw new Error("Invalid id. Id must be a number.");
    }

    return this.http
      .get<Medicine>(
        `${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.BASE}/${id}`
      )
      .pipe(
        catchError((error: HttpErrorResponse) =>
          this.handleApiErrorService.handleError(error)
        )
      );
  }

  getMedicinesByCategory(categoryId: number): Observable<Medicine[]> {
    const url = `${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.GET_BY_CATEGORY}/${categoryId}`;

    return this.http.get<Medicine[]>(url);

  }
  addMedicine(formMedicine: FormData): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.BASE}`, formMedicine);
  }

  deleteMedicine(id: number): Observable<any> {
    return this.http.delete<any>(`${environment.apiUrl}/${Constant.API_ENDPOINT.MEDICINE.BASE}/${id}`)
    .pipe(
      catchError((error: HttpErrorResponse) =>
        this.handleApiErrorService.handleError(error)
      )
    );
  }
}
