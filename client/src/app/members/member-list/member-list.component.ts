import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/Pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
   selector: 'app-member-list',
   templateUrl: './member-list.component.html',
   styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
   members: Member[];
   pagination: Pagination;
   userParams: UserParams;
   user: User;

   constructor(
      private memberService: MembersService,
      private accountService: AccountService
   ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
         this.user = user;
         this.userParams = new UserParams(user);
      });
   }

   ngOnInit(): void {
      this.loadMembers();
   }

   loadMembers() {
      this.memberService.getMembers(this.userParams).subscribe((res) => {
         this.members = res.result;
         this.pagination = res.pagination;
      });
   }

   pageChanged(e: any) {
      // console.log(e); // {page: 2, itemsPerPage: 5}
      this.userParams.pageNumber = e.page;
      this.loadMembers();
   }
}
