import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//toastr
import { ToastrModule } from 'ngx-toastr';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      // ngx-bootstrap
      BsDropdownModule.forRoot(),
      //toastr
      ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
   ],
   exports: [BsDropdownModule, ToastrModule],
})
export class SharedModule {}
