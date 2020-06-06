import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
 

@Injectable({
  providedIn: 'root',
})
export class ClosedDatesService
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
    let url = this.baseUrl + 'api/closeddates';
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

  get<ClosedDate>(id): Observable<ClosedDate> {
    let url = this.baseUrl + "api/closeddates/" + id;
    return this.http.get<ClosedDate>(url);
  }

  put<ClosedDate>(item): Observable<ClosedDate> {
    let url = this.baseUrl + "api/closeddates/" + item.id;
    return this.http.put<ClosedDate>(url, item);
  }

  post<ClosedDate>(item): Observable<ClosedDate> {
    let url = this.baseUrl + "api/closeddates/" + item.id;
    return this.http.post<ClosedDate>(url, item);
  }

  isDupeField(locationId, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("locationId", locationId)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/closeddates/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
