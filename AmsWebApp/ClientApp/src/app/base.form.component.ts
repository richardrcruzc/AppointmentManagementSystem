import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  template: ''
})
export class BaseFormComponent {
  // the form model
  form: FormGroup;
  constructor() { }


  // retrieve a FormControl
  getControl(name: string) {
    return this.form.get(name);
  }
  // returns TRUE if the FormControl is valid
  isValid(name: string) {
    let e = this.getControl(name);
    return e && e.valid;
  }
  // returns TRUE if the FormControl has been changed
  isChanged(name: string) {
    let e = this.getControl(name);
    return e && (e.dirty || e.touched);
  }
  // returns TRUE if the FormControl is raising an error,
  // i.e. an invalid state after user changes
  hasError(name: string) {
    let e = this.getControl(name);
    return e && (e.dirty || e.touched) && e.invalid;
  }
  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    let pass = group.get('password').value;
    let confirmPass = group.get('confirmPassword').value;

    return pass === confirmPass ? null : { notSame: true }
  }
}
