import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn, FormArray } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
 
import { Title } from '@angular/platform-browser';

import { BaseFormComponent } from '../base.form.component';
   
import { MessageService } from './message.service';
import { CustomerMessage } from './message';



@Component({
  selector: 'app-message-edit',
  templateUrl: './message-edit.component.html',
  styleUrls: ['./message-edit.component.css']
})
export class MessageEditComponent 
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the CustomerMessage object to edit or create
  message: CustomerMessage;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new message,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private messageService: MessageService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    this.form = this.fb.group({
      textMessage: ['',
        Validators.required 
      ],
      recipient: ['',
          Validators.required, 
      ],
      method: ['', Validators.required
      ],
      typeMessage: ['', Validators.required
      ],
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
    this.form.get("textMessage")!.valueChanges
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

      // fetch the message from the server
      this.messageService.get<CustomerMessage>(this.id).subscribe(result => {
        this.message = result;
        this.title = "Edit - " + this.message.textMessage;

        // update the form with the message value
        this.form.patchValue(this.message);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new CustomerMessage";
    }
  }

  onSubmit() {

    let message = (this.id) ? this.message : <CustomerMessage>{};

    message.textMessage = this.form.get("textMessage").value;
    message.location = this.form.get("location").value;
    message.method = this.form.get("method").value;
    message.recipient = this.form.get("recipient").value;
    message.typeMessage = this.form.get("typeMessage").value;
    
    

    if (this.id) {
      // EDIT mode

      this.messageService
        .put<CustomerMessage>(message)
        .subscribe(result => {

          console.log("CustomerMessage " + message.id + " has been updated.");

          // go back to messages view
          this.router.navigate(['/messages']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.messageService
        .post<CustomerMessage>(message)
        .subscribe(result => {

          console.log("CustomerMessage " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/messages']);
        }, error => { this.title = error.message; console.log(error); });
    }
  }

  //isDupeField(fieldName: string): AsyncValidatorFn {
  //  return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
  //    let messageId = (this.id) ? this.id : "0";

  //    return this.messageService.isDupeField(
  //      messageId,
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
