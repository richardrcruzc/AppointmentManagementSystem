import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
 

@Injectable({
  providedIn: 'root',
})
export class MessageService
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
    let url = this.baseUrl + 'api/messages';
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

  get<CustomerMessage>(id): Observable<CustomerMessage> {
    let url = this.baseUrl + "api/messages/" + id;
    return this.http.get<CustomerMessage>(url);
  }

  put<CustomerMessage>(item): Observable<CustomerMessage> {
    let url = this.baseUrl + "api/messages/" + item.id;
    return this.http.put<CustomerMessage>(url, item);
  }

  post<CustomerMessage>(item): Observable<CustomerMessage> {
    let url = this.baseUrl + "api/messages/" + item.id;
    return this.http.post<CustomerMessage>(url, item);
  }

  isDupeField(locationId, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("locationId", locationId)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/messages/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
