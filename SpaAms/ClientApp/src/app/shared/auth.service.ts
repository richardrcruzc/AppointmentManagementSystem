import { Injectable, Inject } from '@angular/core';
import { User } from './user';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { LogedInModel } from '../components/signin/login';

@Injectable({
  providedIn: 'root'
})

export class AuthService { 
  headers = new HttpHeaders().set('Content-Type', 'application/json');
  
  

  constructor(
    private http: HttpClient,
    public router: Router,
    @Inject('BASE_URL') private baseUrl: string
  ) {
   
  }


  getCurrentUser(): Observable<any> {
   

    if (!this.isLoggedIn) {
      return  new Observable<null>(); }

    let api = this.baseUrl + 'api/currentuser' ;
    return this.http.post(api, { headers: this.headers }).pipe(
      map((res: Response) => {
        return res || {};
      }),
      catchError(this.handleError)
    )
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getCurrentUser().pipe(map(u => !!u));
  }

  public isEmailExist(email: string): Observable<boolean> {
    let api = this.baseUrl + 'api/isValidEmail/' + email;
    return this.http.post(api, { headers: this.headers }).pipe(
      map((res: boolean) => {
        return res;
      }),
      catchError(this.handleError)
    )
  }
 



  // Sign-up
  signUp(user: User): Observable<any> {
    let api = this.baseUrl + 'api/userregister';
    return this.http.post(api, user)
      .pipe(
        catchError(this.handleError)
      )
  }

  // Sign-in
  signIn(user: User) {
    let api = this.baseUrl + 'api/token'
    return this.http.post<any>(api, user)
      .subscribe((res: any) => {

        localStorage.removeItem('user_name');
        localStorage.removeItem('access_token');
        localStorage.removeItem('user_role');

        
        localStorage.setItem('access_token', res.token);
        localStorage.setItem('user_name', res.userName);
        localStorage.setItem('user_role', res.userRole);

        if (this.isLoggedIn===true) {
          this.router.navigate(['/']);
        }
        
      }),
      catchError(this.handleError)
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  get isLoggedIn(): boolean {
    let authToken = localStorage.getItem('access_token');
    return (authToken !== null && authToken !== 'undefined') ? true : false;
  }
   


  doLogout() {
    localStorage.removeItem('access_token');
     
      this.router.navigate(['/log-in']);
  
  }

  // User profile
  getUserProfile(id): Observable<any> {
    if (this.isLoggedIn !== true) {
      return new Observable<null>();
    }

    let api = this.baseUrl + 'api/user-profile/'+id;    
    return this.http.get(api, { headers: this.headers }).pipe(
      map((res: Response) => {
        return res || {}
      }),
      catchError(this.handleError)
    )
  }

  // Error 
  handleError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      msg = error.error.message;
    } else {
      // server-side error
      msg = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(msg);
  }
}
