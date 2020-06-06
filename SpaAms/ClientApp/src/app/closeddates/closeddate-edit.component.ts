import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn, FormArray } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
 
import { Title } from '@angular/platform-browser';

import { BaseFormComponent } from '../base.form.component';
import { ClosedDate } from './closeddate';
import { ClosedDatesService } from './closeddate.service';
   
 



@Component({
  selector: 'app-closeddate-edit',
  templateUrl: './closeddate-edit.component.html',
  styleUrls: ['./closeddate-edit.component.css']
})
export class ClosedDateEditComponent
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the ClosedDate object to edit or create
  closeddate:  ClosedDate;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new closeddate,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private closeddateService: ClosedDatesService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    this.form = this.fb.group({
      from: ['',
        Validators.required 
      ],
      to: ['',
          Validators.required, 
      ],
      description: ['', Validators.required
      ] ,
      location: ['', Validators.required
      ] 
    });

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
    this.form.get("description")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("Description has bee loaded with initial values");
        }
        else {
          this.log("Description was updated by the user");
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

      // fetch the closeddate from the server
      this.closeddateService.get<ClosedDate>(this.id).subscribe(result => {
        this.closeddate = result;
        this.title = "Edit - " + this.closeddate.description;

        // update the form with the closeddate value
        this.form.patchValue(this.closeddate);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new ClosedDate";
    }
  }

  onSubmit() {

    let closeddate = (this.id) ? this.closeddate : <ClosedDate>{};

    closeddate.from = this.form.get("from").value;
    closeddate.location = this.form.get("location").value;
    closeddate.to = this.form.get("to").value;
    closeddate.description = this.form.get("description").value; 
    
    

    if (this.id) {
      // EDIT mode

      this.closeddateService
        .put<ClosedDate>(closeddate)
        .subscribe(result => {

          console.log("ClosedDate " + closeddate.id + " has been updated.");

          // go back to closeddates view
          this.router.navigate(['/closeddates']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.closeddateService
        .post<ClosedDate>(closeddate)
        .subscribe(result => {

          console.log("ClosedDate " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/closeddates']);
        }, error => { this.title = error.closeddate; console.log(error); });
    }
  }

  //isDupeField(fieldName: string): AsyncValidatorFn {
  //  return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
  //    let closeddateId = (this.id) ? this.id : "0";

  //    return this.closeddateService.isDupeField(
  //      closeddateId,
  //      fieldName,
  //      control.value)
  //      .pipe(map(result => {
  //        return (result ? { isDupeField: true } : null);
  //      }));
  //  }
    
  //}

  //get aliases() {
  //  return this.form.get('aliases') as FormArray;
  //}
  //addAlias() {
  //  this.aliases.push(this.fb.control(''));
  //}

}
