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

import { UserService } from './user.service';


@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // theUser object to edit or create
  user: User;

  //userRoles: UserRole[] = [];


  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new user,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private userService: UserService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    // Admin, Employee, Customer
    //this.userRoles.push({ id: 1, roleName: 'Admin' });
    //this.userRoles.push({ id: 2, roleName: 'Employee' });
    //this.userRoles.push({ id: 3, roleName: 'Customer' })


    this.form = this.fb.group({
      lastName: ['',
        Validators.required
      ],
      firstName: ['',
        Validators.required,
      ],
      mobile: ['', Validators.required
      ],
      email: ['', Validators.required,
        this.isDupeField("email")  
      ],
      title: ['', Validators.required
      ],
      briefCv: ['', Validators.required
      ] ,
      active: false,
      canBook: false,
      role:[''],
      //aliases: this.fb.array([
      //  this.fb.control('')
      //])
    } );

    // react to form changes
    this.form.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("Form Model has been loaded.");
        }
        else {
          this.log("Form was updated by the user.");
        }
      });
    this.form.get("lastName")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("lastname has bee loaded with initial values");
        }
        else {
          this.log("lastname was updated by the user");
        }

      });
     
    this.loadData();
  }

  log(str: string) {
    this.activityLog += "["
      + new Date().toLocaleString()
      + "] " + str + "<br />";
  }


  loadData() {
     
  

    // retrieve the ID from the 'id'
    this.id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (this.id) {
      // EDIT MODE

      // fetch the user from the server
      this.userService.get<User>(this.id).subscribe(result => {
        this.user = result;
        this.title = "Edit User- " + this.user.firstName+" " + this.user.lastName;
         

        // update the form with the user value
        this.form.patchValue(this.user);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new User";
    }
  }

  onSubmit() {

    let user = (this.id) ? this.user : <User>{};

    user.lastName = this.form.get("lastName").value;
    user.firstName = this.form.get("firstName").value;
    user.mobile = this.form.get("mobile").value;
    user.email = this.form.get("email").value; 
    user.title = this.form.get("title").value;
    user.briefCv = this.form.get("briefCv").value;  
    user.active = this.form.get("active").value;
    user.canBook = this.form.get("canBook").value;   
     
    if (this.id) {
      // EDIT mode

      this.userService
        .put<User>(user)
        .subscribe(result => {

          console.log("User " + user.id + " has been updated.");

          // go back to locations view
          this.router.navigate(['/users-profiles']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode

      
      this.userService
        .post<User>(user)
        .subscribe(result => {

          console.log("User " + result.id + " has been created.");

          // go back to profiles view
          this.router.navigate(['/users-profiles']);
        }, error =>
        {
            this.title = error.message;
            console.log(error);
        });
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => { 

      
      let userId = (this.id) ? this.id : "0";

      return this.userService.isDupeField(
        userId,
        fieldName,
        control.value)
        .pipe(map(result => {
          return (result ? { isDupeField: true } : null);
        }));
    }

  }

  //get aliases() {
  //  return this.form.get('aliases') as FormArray;
  //}
  //addAlias() {
  //  this.aliases.push(this.fb.control(''));
  //}

}
