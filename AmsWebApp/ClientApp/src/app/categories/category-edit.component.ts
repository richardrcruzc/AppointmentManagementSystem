import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs'; 

import { BaseFormComponent } from '../base.form.component';
  
import { Category } from './category';
import { CategoryService } from './category.service';



@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html'
})
export class CategoryEditComponent 
  extends BaseFormComponent {
  // the view title
  title: string;

  // the form model
  form: FormGroup;

  // the Category object to edit or create
  category: Category;

  // the city object id, as fetched from the active route:
  // It's NULL when we're adding a new category,
  // and not NULL when we're editing an existing one.
  id?: number;

  //activity log (for debugging purpose)
  activityLog: string = "";
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,   
    private categoryService: CategoryService) {
    super();
    this.loadData();
  }

  ngOnInit() {
    this.form = this.fb.group({
      description: ['',
        Validators.required,
        this.isDupeField("description")
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

      // fetch the category from the server
      this.categoryService.get<Category>(this.id).subscribe(result => {
        this.category = result;
        this.title = "Edit - " + this.category.description;

        // update the form with the category value
        this.form.patchValue(this.category);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new Category";
    }
  }

  onSubmit() {

    let category = (this.id) ? this.category : <Category>{};

    category.description = this.form.get("description").value;
    
    

    if (this.id) {
      // EDIT mode

      this.categoryService
        .put<Category>(category)
        .subscribe(result => {

          console.log("Category " + category.id + " has been updated.");

          // go back to locations view
          this.router.navigate(['/categories']);
        }, error => console.log(error));
    }
    else {
      // ADD NEW mode
      this.categoryService
        .post<Category>(category)
        .subscribe(result => {

          console.log("Category " + result.id + " has been created.");

          // go back to cities view
          this.router.navigate(['/categories']);
        }, error => { this.title = error.message; console.log(error); });
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
       
      let Id = (this.id) ? this.id : "0";

      return this.categoryService.isDupeField(
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
