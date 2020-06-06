import { Component } from '@angular/core';
import { AuthService } from './shared/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(public authService: AuthService) { }

  logout() {
    this.authService.doLogout()
  }
}
