import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
 
import { ClientI } from './client';

@Injectable({
  providedIn: 'root',
})
export class ClientService
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
    let url = this.baseUrl + 'api/clients';
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

  get<ClientI>(id): Observable<ClientI> {
    let url = this.baseUrl + "api/clients/" + id;
    return this.http.get<ClientI>(url);
  }

  put<ClientI>(item): Observable<ClientI> {
    let url = this.baseUrl + "api/clients/" + item.id;
    return this.http.put<ClientI>(url, item);
  }

  post<ClientI>(item): Observable<ClientI> {
    let url = this.baseUrl + "api/clients" ;
    return this.http.post<ClientI>(url, item);
  }

  isDupeField(Id, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("Id", Id)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/clients/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
