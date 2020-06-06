import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormGroup, FormBuilder, Validators, AbstractControl,
  AsyncValidatorFn, FormArray
} from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';


import { User } from '../../shared/User';

import { Title } from '@angular/platform-browser';

import { BaseFormComponent } from '../../base.form.component';

import { UserService } from '../user-profile/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})

export class SignupComponent
  extends BaseFormComponent  {

  form: FormGroup;

  user: User;


  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private userService: UserService) {
    super();
   
  }

  ngOnInit() {

    this.form = this.fb.group({
      firstName: ['',
        Validators.required],
      lastName: ['',
        Validators.required],
      email: ['',
        Validators.required,
        this.isDupeField("email")]  ,
      mobile: ['',
        Validators.required],
      password: ['',
        Validators.required],
      confirmPassword: ['',
        Validators.required]
    }, { validator: this.checkPasswords });
     
  }
   
  registerUser() {
    console.log(this.form.value);
    this.userService.post<User>(this.form.value).subscribe((res) => {
      if (res) {
        
        this.form.reset()
        this.router.navigate(['/log-in']);
      }
    })
  }


  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => { 
      let userId =   "0";

      return this.userService.isDupeField(
        userId,
        fieldName,
        control.value)
        .pipe(map(result => {
          return (result ? { isDupeField: true } : null);
        }));
    }

  }
}
