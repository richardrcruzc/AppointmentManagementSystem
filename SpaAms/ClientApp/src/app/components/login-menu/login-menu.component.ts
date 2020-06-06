import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/auth.service';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
 

  constructor(private authorizeService: AuthService) {
  this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getCurrentUser().pipe(map(u => u && u.email)); }

  ngOnInit() {

    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getCurrentUser().pipe(map(u => u && u.email));
  }


  logOut() {
    this.authorizeService.doLogout();
    this.ngOnInit();
  }
   
}
