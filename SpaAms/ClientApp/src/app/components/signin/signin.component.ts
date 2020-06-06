import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn, FormControl } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

import { BaseFormComponent } from '../../base.form.component';
import { AuthService } from '../../shared/auth.service';
import { LoginModel } from './login';
 
 

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})

export class SigninComponent   extends BaseFormComponent{
  email: string;
  title: string = '';
  signinForm: FormGroup;
  loginModel: LoginModel;

  // Activity Log (for debugging purposes)
  activityLog: string = '';

  constructor(
    private activateRoute: ActivatedRoute,
    public fb: FormBuilder,
    public authService: AuthService,
    public router: Router
  ) {
    super();

    //  this.signinForm = this.fb.group({
    //  email: ['', Validators.required],
    //  password: ['', Validators.required],
    //  rememberMe: ['']
    //});

  
 
  }
   
  ngOnInit() {
  
    this.signinForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email], this.isValidEmail()),
      password: new FormControl('', Validators.required),
      rememberMe: new FormControl(false)
    });

    // react to form changes
    this.signinForm.valueChanges
      .subscribe(val => {
        if (!this.signinForm.dirty) {
          this.log("Form Model has been loaded.");
        }
        else {
          this.log("Form was updated by the user.");
        }
      });

    // react to changes in the form.name control
    this.signinForm.get("email")!.valueChanges
      .subscribe(val => {
        if (!this.signinForm.dirty) {
          this.log("email has been loaded with initial values.");
        }
        else {
          this.log("email was updated by the user.");
        }
      });

  }
  log(str: string) {
    this.activityLog += "["
      + new Date().toLocaleString()
      + "] " + str + "<br />";
  }

  loginUser() { 
    this.title = '';
    this.authService.signIn(this.signinForm.value)

    this.title = "Unable to log in <br /> please check you email and password and try again.";
  }
  
  isValidEmail(): AsyncValidatorFn {
   
  
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      let email = control.value;

      
      return this.authService.isEmailExist(email)
        .pipe(map(result => {
        
          
          return (result ? null: { isValidEmail: true });
        }));
    }
  }
}
