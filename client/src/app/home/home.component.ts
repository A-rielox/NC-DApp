import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'app-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
   registerMode = false;

   constructor() {}

   ngOnInit(): void {}

   registerToggle() {
      this.registerMode = !this.registerMode;
   }

   cancelRegisterMode(event: boolean) {
      console.log('event-----', event);
      this.registerMode = event;
   }
}

// getUsers() {
//    this.http.get('https://localhost:5001/api/users').subscribe(
//       (res) => {
//          console.log(res);
//          this.users = res;
//       },
//       (err) => console.log(err)
//    );
// }
//
// o
//
// getUsers() {
//    this.http.get('https://localhost:5001/api/users').subscribe({
//       next: (response) => (this.users = response),
//       error: (error) => console.log(error),
//    });
// }
