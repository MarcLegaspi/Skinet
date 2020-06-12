import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup, AsyncValidatorFn } from '@angular/forms';
import { timer, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: [];
  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  onSubmit() {
    console.log(this.registerForm.value);
    this.accountService.register(this.registerForm.value).subscribe(res => {
        this.router.navigateByUrl('/shop');
    }, error => {
      this.errors = error.errors;
      console.log(error);
    })
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      displayName: ['', Validators.required],
      password: [null, Validators.required],
      email: ['',
             [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
             [this.validateEmailNotTaken()]
      ]
    }
    );
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if(!control.value) {
            return of(null);
          }

          return this.accountService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          )
        })
      );
    };
  }
}
