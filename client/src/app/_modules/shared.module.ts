import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
//toastr
import { ToastrModule } from 'ngx-toastr';

// galleria
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
   declarations: [],
   imports: [
      CommonModule,
      NgxGalleryModule,
      FileUploadModule,
      // ngx-bootstrap
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      //toastr
      ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
   ],
   exports: [
      BsDropdownModule,
      ToastrModule,
      TabsModule,
      NgxGalleryModule,
      FileUploadModule,
   ],
})
export class SharedModule {}
