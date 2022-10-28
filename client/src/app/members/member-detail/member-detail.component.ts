import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
   selector: 'app-member-detail',
   templateUrl: './member-detail.component.html',
   styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
   member: Member;
   // galleryOptions: NgxGalleryOptions[];
   // galleryImages: NgxGalleryImage[];

   constructor(
      private memberService: MembersService,
      private route: ActivatedRoute
   ) {}

   ngOnInit(): void {
      this.loadMember();

      // this.galleryOptions = [
      //    {
      //       width: '500px',
      //       height: '500px',
      //       imagePercent: 100,
      //       thumbnailsColumns: 4,
      //       imageAnimation: NgxGalleryAnimation.Slide,
      //       preview: false,
      //    },
      // ];
   }

   // getImages(): NgxGalleryImage[] {
   //    const imageUrls = [];
   //    for (const photo of this.member.photos) {
   //       imageUrls.push({
   //          small: photo?.url,
   //          medium: photo?.url,
   //          big: photo?.url,
   //       });
   //    }
   //    return imageUrls;
   // }

   loadMember() {
      // el nombre del member q viene en el param
      const memberName = this.route.snapshot.paramMap.get('username');

      this.memberService.getMember(memberName).subscribe((member) => {
         this.member = member;
      });
   }
}
