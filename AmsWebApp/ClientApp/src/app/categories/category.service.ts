import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
import { Category } from './category';



@Injectable({
  providedIn: 'root',
})
export class CategoryService
  extends BaseService {
  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    super(http, baseUrl);
  }

  getData<ApiResult>(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string,
    filterQuery: string
  ): Observable<ApiResult> {
    let url = this.baseUrl + 'api/categories';
    let params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);

    if (filterQuery) {
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);
    }

    return this.http.get<ApiResult>(url, { params });
  }

  get<Category>(id): Observable<Category> {
    let url = this.baseUrl + "api/categories/" + id;
    return this.http.get<Category>(url);
  }

  put<Category>(item): Observable<Category> {
    let url = this.baseUrl + "api/categories/" + item.id;
    return this.http.put<Category>(url, item);
  }

  post<Category>(item): Observable<Category> {
    let url = this.baseUrl + "api/categories" ;
    return this.http.post<Category>(url, item);
  }

  isDupeField(Id, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("Id", Id)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/categories/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
