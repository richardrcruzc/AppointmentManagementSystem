import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './shared/User'; 

@Injectable({
  providedIn: 'root'
})

export class JwtService {
  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {}

  login(username: string, password: string) {
    let url = this.baseUrl + 'api/token'; 
    return this.http.post<User>(url, { username, password })
      .subscribe(result => {
        localStorage.setItem('access_token', '');
      }, error => { console.log(error); });

    //return this.httpClient.post<{ access_token: string }>('https://localhost:44350/api/token/', { username, password })
    //  .pipe(tap(res => {
    //  localStorage.setItem('access_token', res.access_token);
    //}))
  }

  //register(username: string, password: string) {
  //  return this.httpClient.post<{ access_token: string }>('https://localhost:44350/api/authenticate/', { username, password })
  //    .pipe(tap(res => {
  //      this.login(username, password)
  //  }))
  //}
  //logout() {
  //  localStorage.removeItem('access_token');
  //}
  public get loggedIn(): boolean {
    return localStorage.getItem('access_token') !== null;
  }
}

