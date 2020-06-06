import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs'; 

import { BaseFormComponent } from '../base.form.component';
import { ClientI, SendBy } from './client';
import { ClientService } from './client.service';
   



@Component({
  selector: 'app-client-edit',
  templateUrl: './client-edit.component.html'
})
export class ClientEditComponent 
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the ClientI object to edit or create
  client: ClientI;

  sendBys: SendBy[] = [];


  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new client,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private clientService: ClientService) {
    super();
    this.loadData();
  }

  ngOnInit() {

    this.sendBys.push({ sendByName:'Email'});
    this.sendBys.push({ sendByName: 'Phone'});


    this.form = this.fb.group({
      lastName: ['',
        Validators.required
      ],
      firstName: ['',
        Validators.required
      ],
      mobile: ['',
        Validators.required
      ],
      email: ['',
        Validators.required,
        this.isDupeField("email")
      ] ,
      active:false,
      sendNotificationBy: ['', Validators.required],
      acceptsMarketingNotifications: false,
      photo: ['']
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
    this.form.get("lastName")!.valueChanges
      .subscribe(val => {
        if (!this.form.dirty) {
          this.log("lastName has bee loaded with initial values");
        }
        else {
          this.log("lastName was updated by the user");
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

      // fetch the client from the server
      this.clientService.get<ClientI>(this.id).subscribe(result => {
        this.client = result;
        this.title = "Edit - " + this.client.lastName;

        // update the form with the client value
        this.form.patchValue(this.client);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new ClientI";
    }
  }

  onSubmit() {

    let client = (this.id) ? this.client : <ClientI>{};

    client.lastName = this.form.get("lastName").value;
    client.firstName = this.form.get("firstName").value;
    client.mobile = this.form.get("mobile").value;
    client.email = this.form.get("email").value;
    client.active = this.form.get("active").value;
    client.sendNotificationBy = this.form.get("sendNotificationBy").value;
    client.acceptsMarketingNotifications = this.form.get("acceptsMarketingNotifications").value;
    client.photo = this.form.get("lastName").value;
    
    

    if (this.id) {
      // EDIT mode

      this.clientService
        .put<ClientI>(client)
        .subscribe(result => {

          console.log("ClientI " + client.id + " has been updated.");

          // go back to locations view
          this.router.navigate(['/clients']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.clientService
        .post<ClientI>(client)
        .subscribe(result => {

          console.log("ClientI " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/clients']);
        }, error => { this.title = error.message; console.log(error); });
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
      let Id = (this.id) ? this.id : "0";

      return this.clientService.isDupeField(
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
