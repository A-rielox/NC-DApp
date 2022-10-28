import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
/* 
      el spinner de loading lo manejo con interceptor (LoadingInterceptor)
      lo puse en app-component.html
*/
@Component({
   selector: 'app-member-edit',
   templateUrl: './member-edit.component.html',
   styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
   @ViewChild('editForm') editForm: NgForm;

   member: Member;
   user: User; // tiene solo username y token

   // 🟡 xsi quiere cerrar la pestaña o tipear otra url o algo asi
   // @HostListener('window:beforeunload', ['$event']) unloadNotification(
   //    $event: any
   // ) {
   //    $event.returnValue = true;
   // }

   constructor(
      private accountService: AccountService,
      private memberService: MembersService,
      private toastrService: ToastrService
   ) {
      this.accountService.currentUser$
         .pipe(take(1))
         .subscribe((user) => (this.user = user));
   }

   ngOnInit(): void {
      this.loadMember();
   }

   loadMember() {
      this.memberService
         .getMember(this.user.username)
         .subscribe((member) => (this.member = member));
   }

   updateMember() {
      console.log(this.member);

      this.memberService.updateMember(this.member).subscribe(() => {
         this.toastrService.success('Profile updated successfully.');

         // 🟡 p' resetear los valores de "dirty, touched y esos" pero mantener los valores de los campos
         this.editForm.reset(this.member);
      });
   }
}
