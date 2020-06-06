import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { BaseService, ApiResult } from '../../base.service';
import { Observable } from 'rxjs';

import { User } from '../../shared/user';

@Injectable({
  providedIn: 'root',
})
export class UserService
  extends BaseService {
  headers = new HttpHeaders().set('Content-Type', 'application/json');
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
    let url = this.baseUrl + 'api/getallusers';
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

  get<User>(id): Observable<User> {
    let url = this.baseUrl + "api/user-profile/" + id;
    return this.http.get<User>(url);
  }

  put<User>(item): Observable<User> {
    let url = this.baseUrl + "api/usereupdate/" + item.id;
    return this.http.put<User>(url, item, { headers: this.headers });
  }

  post<User>(item): Observable<User> {
    let url = this.baseUrl + "api/Userregister" ; 
    console.log(item);
    
    return this.http.post<User>(url,  item, { headers: this.headers } ); 
  }

  isDupeField(userId, fieldName, fieldValue): Observable<boolean> {
    let params = new HttpParams()
      .set("userId", userId)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    let url = this.baseUrl + "api/IsDupeField";
    return this.http.post<boolean>(url, null, { params });
  }



  getCurrentUser(): Observable<any> {
    let authToken = localStorage.getItem('access_token');
    if (authToken === null || authToken === 'undefined') {
      return new Observable<User>();
    
    }


    let url = this.baseUrl + 'api/currentuser';
    return this.http.post<User>(url, { headers: this.headers });
  }
}
