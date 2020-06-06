import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';

import { Location } from './location';

@Injectable({
  providedIn: 'root',
})
export class LocationService
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
    var url = this.baseUrl + 'api/locations';
    var params = new HttpParams()
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

  get<Location>(id): Observable<Location> {
    var url = this.baseUrl + "api/locations/" + id;
    return this.http.get<Location>(url);
  }

  put<Location>(item): Observable<Location> {
    var url = this.baseUrl + "api/locations/" + item.id;
    return this.http.put<Location>(url, item);
  }

  post<Location>(item): Observable<Location> {
    var url = this.baseUrl + "api/locations/" + item.id;
    return this.http.post<Location>(url, item);
  }

  isDupeField(locationId, fieldName, fieldValue): Observable<boolean> {
    var params = new HttpParams()
      .set("locationId", locationId)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    var url = this.baseUrl + "api/locations/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }
}
