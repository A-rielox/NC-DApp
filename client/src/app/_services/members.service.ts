import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/Pagination';
import { UserParams } from '../_models/userParams';

@Injectable({
   providedIn: 'root',
})
export class MembersService {
   baseUrl = environment.apiUrl;
   members: Member[] = [];

   constructor(private http: HttpClient) {}

   getMembers(userParams: UserParams) {
      // if (this.members.length > 0) return of(this.members);
      let params = this.getPaginationHeaders(
         userParams.pageNumber,
         userParams.pageSize
      );

      params = params.append('minAge', userParams.minAge.toString());
      params = params.append('maxAge', userParams.maxAge.toString());
      params = params.append('gender', userParams.gender);

      return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params);
   }

   getMember(username: string) {
      const member = this.members.find((m) => m.username === username);
      if (member) return of(member);

      return this.http.get<Member>(this.baseUrl + 'users/' + username);
   }

   updateMember(member: Member) {
      return this.http.put(this.baseUrl + 'users', member).pipe(
         map(() => {
            const index = this.members.indexOf(member);
            this.members[index] = member;
         })
      );
   }

   setMainPhoto(photoId: number) {
      return this.http.put(
         this.baseUrl + 'users/set-main-photo/' + photoId,
         {}
      );
   }

   deletePhoto(photoId: number) {
      return this.http.delete(
         this.baseUrl + 'users/delete-photo/' + photoId,
         {}
      );
   }

   ///////////////

   private getPaginatedResult<T>(url, params) {
      // prettier-ignore
      const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

      return (
         this.http
            // al pasarle el {observe: ...} ahora en lugar de devolver el body ( del get method ) me va a devolver toda la response
            .get<T>(url, { observe: 'response', params })
            .pipe(
               map((res) => {
                  paginatedResult.result = res.body;

                  if (res.headers.get('Pagination') !== null) {
                     paginatedResult.pagination = JSON.parse(
                        res.headers.get('Pagination')
                     );
                  }

                  return paginatedResult;
               })
            )
      );
   }

   private getPaginationHeaders(pageNumber: number, pageSize: number) {
      let params = new HttpParams();

      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());

      return params;
   }
}
