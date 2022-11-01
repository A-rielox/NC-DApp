import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
   AbstractControl,
   FormBuilder,
   FormControl,
   FormGroup,
   ValidatorFn,
   Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
   selector: 'app-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
   @Output() cancelRegister = new EventEmitter();
   registerForm: FormGroup;
   maxDate: Date;
   validationErrors: string[] = [];

   constructor(
      private accountService: AccountService,
      private toastr: ToastrService,
      private fb: FormBuilder,
      private router: Router
   ) {}

   ngOnInit(): void {
      this.initializeForm();
      // para no permitir menores de 18
      this.maxDate = new Date();
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
   }

   initializeForm() {
      // prettier-ignore
      this.registerForm = this.fb.group({
         gender: ['male'],
         username: ['', Validators.required],
         knownAs: ['', Validators.required],
         dateOfBirth: ['', Validators.required],
         city: ['', Validators.required],
         country: ['', Validators.required],
         password: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
         confirmPassword: ['',[Validators.required, this.matchValues('password')]],
      });

      // por si cambia el password despues de poner el confirmPassword y pasar la validacion
      this.registerForm.controls.password.valueChanges.subscribe(() => {
         this.registerForm.controls.confirmPassword.updateValueAndValidity();
      });
   }

   matchValues(matchTo: string): ValidatorFn {
      return (control: AbstractControl) => {
         // si SI es = se devuelve null
         // control?.value es el valor del ctrl donde pongo este validator
         return control?.value === control?.parent?.controls[matchTo].value
            ? null
            : { isMatching: true };
      };
   }

   register() {
      this.accountService.register(this.registerForm.value).subscribe(
         (res) => {
            this.router.navigateByUrl('/members');
         },
         (err) => {
            console.log(err);
            this.validationErrors = err;
         }
      );
   }

   cancel() {
      this.cancelRegister.emit(false);
   }
}
