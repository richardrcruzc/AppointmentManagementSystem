import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    public authService: AuthService,
    public router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const currentUserRole = localStorage.getItem('user_role');
   

    if (this.authService.isLoggedIn === true) {
      //window.alert("Access not allowed!");

      if (route.data.roles && route.data.roles.indexOf(currentUserRole) === -1) {
        // role not authorised so redirect to home page
        this.router.navigate(['/']);
        return false;
      }


      
      return true;
    }
    // not logged in so redirect to login page with the return url
    this.router.navigate(['/log-in'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
