//import { NgModule } from '@angular/core';
//import { CommonModule } from '@angular/common'; 
//import { RouterModule } from '@angular/router'; 
//import { HttpClientModule } from '@angular/common/http';
//import { SigninComponent } from './signin/signin.component';
//import { UsersProfilesComponent } from './user-profile/users-profiles.component';
//import { UserEditComponent } from './user-profile/user-edit.components';
//import { SignupComponent } from './signup/signup.component';
//import { ForgotPasswordComponent } from './signin/forgot-password.component'; 
//import { AuthGuard } from '../shared/auth.guard';
//import { LoginMenuComponent } from './login-menu/login-menu.component';
//import { FormsModule, ReactiveFormsModule } from '@angular/forms';
//import { UserProfileComponent } from './user-profile/user-profile.component';
//@NgModule({
//  imports: [
//    CommonModule,
//    HttpClientModule,
//    FormsModule,
//    ReactiveFormsModule,
//    RouterModule.forChild(
//      [
//        { path: 'log-in', component: SigninComponent },
//        { path: 'users-profiles', component: UsersProfilesComponent, canActivate: [AuthGuard] },
//        { path: 'user-profile/:id', component: UserEditComponent, canActivate: [AuthGuard] },
//        { path: 'user-profile', component: UserEditComponent, canActivate: [AuthGuard] },
//        { path: 'user-signup', component: SignupComponent },
//        { path: 'forgot-password', component: ForgotPasswordComponent }
//      ]
//    ) 
//  ],
//  declarations: [UserProfileComponent,LoginMenuComponent,SigninComponent, UsersProfilesComponent, UserEditComponent, SignupComponent, ForgotPasswordComponent],
//  //exports: [LoginMenuComponent,SigninComponent, UsersProfilesComponent, UserEditComponent, SignupComponent, ForgotPasswordComponent]
//})
//export class ApiAuthorizationModule { }
//# sourceMappingURL=api-authorization.module.js.map