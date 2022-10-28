import { Injectable } from '@angular/core';
import {
   HttpRequest,
   HttpHandler,
   HttpEvent,
   HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
   constructor(private accountService: AccountService) {}

   intercept(
      request: HttpRequest<unknown>,
      next: HttpHandler
   ): Observable<HttpEvent<unknown>> {
      let currentUser: User;

      // .pipe(take(1)) asi, despues de tomar uno, marca como completado y ya no necesito hacer unsubscribe
      // yellow
      // c/vez q no sepa si tengo o no q hacer el unsubscribe puedo ocupar este hack
      // yellow
      this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
         currentUser = user;
      });

      if (currentUser) {
         request = request.clone({
            setHeaders: {
               Authorization: `Bearer ${currentUser.token}`,
            },
         });
      }

      return next.handle(request);
   }
}

/*       
recordar agregarlo en AppModule

providers: [
      {
         provide: HTTP_INTERCEPTORS,
         useClass: ErrorInterceptor,
         multi: true,
      },
      {
         provide: HTTP_INTERCEPTORS,
         useClass: JwtInterceptor,
         multi: true,
      },
   ],

*/
