import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
import { Service } from './service';



@Injectable({
  providedIn: 'root',
})
export class ServiceService
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
    let url = this.baseUrl + 'api/services';
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

  get<Service>(id): Observable<Service> {
    let url = this.baseUrl + "api/services/" + id;
    return this.http.get<Service>(url);
  }

  put<Service>(item): Observable<Service> {
    let url = this.baseUrl + "api/services/" + item.id;
    return this.http.put<Service>(url, item);
  }

  post<Service>(item): Observable<Service> {
    let url = this.baseUrl + "api/services" ;
    return this.http.post<Service>(url, item);
  }

  isDupeField(Id, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("Id", Id)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/services/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
