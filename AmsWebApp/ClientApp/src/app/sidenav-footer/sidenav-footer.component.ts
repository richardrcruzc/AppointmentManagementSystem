import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../shared/auth.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-sidenav-footer',
  templateUrl: './sidenav-footer.component.html'
}) 

export class SideNavFooterComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;


  constructor(private authorizeService: AuthService) {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getCurrentUser().pipe(map(u => u && u.email));
  }

  ngOnInit() {

    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getCurrentUser().pipe(map(u => u && u.email));
  }


  logOut() {
    this.authorizeService.doLogout();
    this.ngOnInit();
  }

}
