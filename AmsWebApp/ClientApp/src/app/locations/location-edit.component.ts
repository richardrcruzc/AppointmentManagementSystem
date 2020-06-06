import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn, FormArray } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';


import { Location, LocationList } from './location';
import { Title } from '@angular/platform-browser';

import { BaseFormComponent } from '../base.form.component';

import { LocationService } from './location.service';
import { ApiResult } from '../base.service';



@Component({
  selector: 'app-location-edit',
  templateUrl: './location-edit.component.html',
  styleUrls: ['./location-edit.component.css']
})
export class LocationEditComponent 
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the Location object to edit or create
  location: Location;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new location,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private locationService: LocationService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    this.form = this.fb.group({
      description: ['',
        Validators.required,
        this.isDupeField("description")
      ],
      contactName: ['',
          Validators.required, 
      ],
      address: ['', Validators.required
      ],
      address1: ['', Validators.required
      ],
      city: ['', Validators.required
      ],
      state: ['', Validators.required
      ],
      zipCode: ['', Validators.required
      ],
      country: ['', Validators.required
      ],
      phone: ['', Validators.required
      ],
      contactEmail: ['', Validators.required
      ],
      active: false,
      confirmation: false,
      reminder: false,
      rescheduling: false,
      thankYou: false,
      cancelation: false,
      noShowUp: false,
      //aliases: this.fb.array([
      //  this.fb.control('')
      //])
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

      // fetch the location from the server
      this.locationService.get<Location>(this.id).subscribe(result => {
        this.location = result;
        this.title = "Edit - " + this.location.description;

        // update the form with the location value
        this.form.patchValue(this.location);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new Location";
    }
  }

  onSubmit() {

    let location = (this.id) ? this.location : <Location>{};

    location.description = this.form.get("description").value;
    location.contactName = this.form.get("contactName").value;
    location.phone = this.form.get("phone").value;
    location.contactEmail = this.form.get("contactEmail").value;
    location.address = this.form.get("address").value;
    location.address1 = this.form.get("address1").value;
    location.city = this.form.get("city").value;
    location.state = this.form.get("state").value;
    location.zipCode = this.form.get("zipCode").value;
    location.country = this.form.get("country").value;
    location.confirmation = this.form.get("confirmation").value;
    location.reminder = this.form.get("reminder").value;
    location.rescheduling = this.form.get("rescheduling").value;
    location.thankYou = this.form.get("thankYou").value;
    location.cancelation = this.form.get("cancelation").value;
    location.noShowUp = this.form.get("noShowUp").value;    
    location.active = this.form.get("active").value;
    

    if (this.id) {
      // EDIT mode

      this.locationService
        .put<Location>(location)
        .subscribe(result => {

          console.log("Location " + location.id + " has been updated.");

          // go back to locations view
          this.router.navigate(['/locations']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.locationService
        .post<Location>(location)
        .subscribe(result => {

          console.log("Location " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/locations']);
        }, error => { this.title = error.message; console.log(error); });
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
      let locationId = (this.id) ? this.id : "0";

      return this.locationService.isDupeField(
        locationId,
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
