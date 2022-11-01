import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
   AbstractControl,
   FormBuilder,
   FormControl,
   FormGroup,
   ValidatorFn,
   Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
   selector: 'app-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
   @Output() cancelRegister = new EventEmitter();
   model: any = {};
   registerForm: FormGroup;

   constructor(
      private accountService: AccountService,
      private toastr: ToastrService,
      private fb: FormBuilder
   ) {}

   ngOnInit(): void {
      this.initializeForm();
   }

   initializeForm() {
      this.registerForm = this.fb.group({
         gender: ['male'],
         username: ['', Validators.required],
         knownAs: ['', Validators.required],
         dateOfBirth: ['', Validators.required],
         city: ['', Validators.required],
         country: ['', Validators.required],
         password: [
            '',
            [
               Validators.required,
               Validators.minLength(4),
               Validators.maxLength(8),
            ],
         ],
         confirmPassword: [
            '',
            [Validators.required, this.matchValues('password')],
         ],
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
      console.log(this.registerForm.value);

      // this.accountService.register(this.model).subscribe(
      //    (res) => {
      //       console.log('res from register', res);
      //       this.cancel();
      //    },
      //    (err) => {
      //       console.log(err);
      //       this.toastr.error(err.error);
      //    }
      // );
   }

   cancel() {
      this.cancelRegister.emit(false);
   }
}
