import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

//toastr
import { ToastrModule } from 'ngx-toastr';

// galleria
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';

//
import { TimeagoModule } from 'ngx-timeago';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      NgxGalleryModule,
      FileUploadModule,
      TimeagoModule.forRoot(),
      // ngx-bootstrap
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      BsDatepickerModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      //toastr
      ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
   ],
   exports: [
      BsDropdownModule,
      ToastrModule,
      TabsModule,
      NgxGalleryModule,
      FileUploadModule,
      BsDatepickerModule,
      PaginationModule,
      ButtonsModule,
      TimeagoModule,
   ],
})
export class SharedModule {}
