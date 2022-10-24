import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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

   constructor(
      private accountService: AccountService,
      private toastr: ToastrService
   ) {}

   ngOnInit(): void {}

   register() {
      this.accountService.register(this.model).subscribe(
         (res) => {
            console.log('res from register', res);
            this.cancel();
         },
         (err) => {
            console.log(err);
            this.toastr.error(err.error);
         }
      );
   }

   cancel() {
      this.cancelRegister.emit(false);
   }
}