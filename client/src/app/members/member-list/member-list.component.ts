import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/Pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
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
   genderList = [
      { value: 'male', display: 'Males' },
      { value: 'female', display: 'Females' },
   ];

   constructor(private memberService: MembersService) {
      this.userParams = this.memberService.getUserParams();
   }

   ngOnInit(): void {
      this.loadMembers();
   }

   loadMembers() {
      // p' 1ro actualizar los params en el service
      this.memberService.setUserParams(this.userParams);

      this.memberService.getMembers(this.userParams).subscribe((res) => {
         this.members = res.result;
         this.pagination = res.pagination;
      });
   }

   resetFilters() {
      this.userParams = this.memberService.resetUserParams();
      this.loadMembers();
   }

   pageChanged(e: any) {
      // console.log(e); // {page: 2, itemsPerPage: 5}
      this.userParams.pageNumber = e.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
   }
}
