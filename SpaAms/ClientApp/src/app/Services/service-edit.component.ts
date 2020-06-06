import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs'; 

import { BaseFormComponent } from '../base.form.component';
import { Service } from './service';
import { ServiceService } from './service.service';
 



@Component({
  selector: 'app-service-edit',
  templateUrl: './service-edit.component.html'
})
export class ServiceEditComponent 
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the Service object to edit or create
  service: Service;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new service,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private serviceService: ServiceService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    this.form = this.fb.group({
      serviceName: ['',
        Validators.required,
        this.isDupeField("serviceName")
      ],
      durationHour: ['',
        Validators.required],
      durationMinute: ['',
        Validators.required],
      price: ['',
        Validators.required],
      photo: [''],
      serviceDescription: ['',
        Validators.required],
      activeStatus: false,
      serviceCategoryId: ['']
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
    this.form.get("serviceName")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("serviceName has bee loaded with initial values");
        }
        else {
          this.log("serviceName was updated by the user");
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

      // fetch the service from the server
      this.serviceService.get<Service>(this.id).subscribe(result => {
        this.service = result;
        this.title = "Edit - " + this.service.serviceName;

        // update the form with the service value
        this.form.patchValue(this.service);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new Service";
    }
  }

  onSubmit() {

    let service = (this.id) ? this.service : <Service>{};

    service.serviceName = this.form.get("serviceName").value;
    service.durationHour = this.form.get("durationHour").value;
    service.durationMinute = this.form.get("durationMinute").value;
    service.price = this.form.get("price").value;
    service.photo = this.form.get("photo").value;
    service.serviceDescription = this.form.get("serviceDescription").value;
    service.activeStatus = this.form.get("activeStatus").value;
    service.serviceCategoryId = +this.form.get("serviceCategoryId").value;
    
    

    if (this.id) {
      // EDIT mode
      console.log(service);
      this.serviceService
        .put<Service>(service)
        .subscribe(result => {

          console.log("Service " + service.id + " has been updated.");

          // go back to locations view
          this.router.navigate(['/services']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.serviceService
        .post<Service>(service)
        .subscribe(result => {

          console.log("Service " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/services']);
        }, error => { this.title = error.message; console.log(error); });
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
      let Id = (this.id) ? this.id : "0";

      return this.serviceService.isDupeField(
        Id,
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
