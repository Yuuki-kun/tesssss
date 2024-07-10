import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError } from 'rxjs';
import { Category } from '../../models/Category';
import { environment } from 'src/environments/environment.development';
import { Constant } from '../../constant/Constant';
import { HandleApiErrorService } from '../handle-api-error.service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient,
              private handleApiErrorService: HandleApiErrorService
  ) { }

  fetchCategoryList(): Observable<Category[]>{
    return this.http.get<Category[]>(`${environment.apiUrl}/${Constant.API_ENDPOINT.CATEGORY.GET_LIST}`)
    .pipe(
      catchError((error: HttpErrorResponse) =>
        this.handleApiErrorService.handleError(error)
      )
    );
  }

  addCategory(category: Category): Observable<Category>{
    return this.http.post<Category>(`${environment.apiUrl}/${Constant.API_ENDPOINT.CATEGORY.BASE}`, category)
    .pipe(
      catchError((error: HttpErrorResponse) =>
        this.handleApiErrorService.handleError(error)
      )
    );
  }
  searchCategory(searchValue: string) : Observable<Category[]>{
    return this.http.get<Category[]>(`${environment.apiUrl}/${Constant.API_ENDPOINT.CATEGORY.SEARCH}${searchValue}`)
    .pipe(
      catchError((error: HttpErrorResponse) =>
        this.handleApiErrorService.handleError(error)
      )
    );
  }
  getCategoryById(categoryId: number): Observable<Category>{
    return this.http.get<Category>(`${environment.apiUrl}/${Constant.API_ENDPOINT.CATEGORY.BASE}/${categoryId}`)
  }

  deleteCategory(categoryId: number): Observable<any>{
    return this.http.delete<any>(`${environment.apiUrl}/${Constant.API_ENDPOINT.CATEGORY.BASE}?id=${categoryId}`)
  }
}
