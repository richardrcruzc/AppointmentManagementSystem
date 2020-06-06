import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, FormControl, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BaseFormComponent } from './base.form.component';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { SigninComponent } from './components/signin/signin.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
 import { UsersProfilesComponent } from './components/user-profile/users-profiles.component';
import { UserEditComponent } from './components/user-profile/user-edit.components';
import { SignupComponent } from './components/signup/signup.component';
import { ForgotPasswordComponent } from './components/signin/forgot-password.component';

import { AuthInterceptor } from './shared/authconfig.interceptor';
import { AuthGuard } from './shared/auth.guard';
import { LocationsComponent } from './locations/locations.component';
import { AngularMaterialModule } from './angular-material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LocationEditComponent } from './locations/location-edit.component';

import { CategoriesComponent } from './categories/categories.component';
import { CategoryEditComponent } from './categories/category-edit.component';
import { ServicesComponent } from './Services/services.component';
import { ServiceEditComponent } from './Services/service-edit.component'; 
import { ClientsComponent } from './Clients/clients.component';
import { ClientEditComponent } from './Clients/client-edit.component';
import { LoginMenuComponent } from './components/login-menu/login-menu.component';
import { ComponentsModule } from './components/components.module'; 
import { MessagesComponent } from './messages/messages.component';
import { MessageEditComponent } from './messages/message-edit.component';
import { ClosedDatesComponent } from './closeddates/closeddates.component'; 
import { ClosedDateEditComponent } from './closeddates/closeddate-edit.component';
 

@NgModule({
  declarations: [
    AppComponent,
    BaseFormComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,    
    LocationsComponent, LocationEditComponent,
    SigninComponent, UserProfileComponent, UsersProfilesComponent, UserEditComponent, SignupComponent, ForgotPasswordComponent,
    CategoriesComponent, CategoryEditComponent,
    ServicesComponent, ServiceEditComponent,
    ClientsComponent, ClientEditComponent, LoginMenuComponent,
    MessagesComponent, MessageEditComponent,
    ClosedDatesComponent, ClosedDateEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },

      { path: 'log-in', component: SigninComponent },
      { path: 'users-profiles', component: UsersProfilesComponent, canActivate: [AuthGuard], data: { roles: ['admin'] }},
      { path: 'user-profile/:id', component: UserEditComponent, canActivate: [AuthGuard], data: { roles: ['admin'] }},
      { path: 'user-profile', component: UserEditComponent, canActivate: [AuthGuard], data:  { roles: ['admin'] }},
      { path: 'my-profile', component: UserProfileComponent, canActivate: [AuthGuard] },
      { path: 'user-signup', component: SignupComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
   
      
   
      { path: 'locations', component: LocationsComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'location/:id', component: LocationEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'location', component: LocationEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},

      { path: 'categories', component: CategoriesComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'category/:id', component: CategoryEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'category', component: CategoryEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},

      { path: 'services', component: ServicesComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'service/:id', component: ServiceEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'service', component: ServiceEditComponent, canActivate: [AuthGuard] },

      { path: 'clients', component: ClientsComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'client/:id', component: ClientEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},
      { path: 'client', component: ClientEditComponent, canActivate: [AuthGuard],data: { roles: ['admin'] }},

      { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
      { path: 'message/:id', component: MessageEditComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
      { path: 'message', component: MessageEditComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },

      { path: 'closeddates', component: ClosedDatesComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
      { path: 'closeddate/:id', component: ClosedDateEditComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
      { path: 'closeddate', component: ClosedDateEditComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },

    ]),
    BrowserAnimationsModule,
    AngularMaterialModule,
    ComponentsModule
  
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
