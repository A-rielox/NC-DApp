import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
   selector: 'app-text-input',
   templateUrl: './text-input.component.html',
   styleUrls: ['./text-input.component.css'],
})
export class TextInputComponent implements ControlValueAccessor {
   @Input() label: string;
   @Input() type = 'text';

   // de esta forma tenemos acceso al control dentro DE ESTE componente ( cuando lo use dentro de las form desde las que lo voy a llamar )
   constructor(@Self() public ngControl: NgControl) {
      this.ngControl.valueAccessor = this;
   }

   writeValue(obj: any): void {
      //
   }
   registerOnChange(fn: any): void {
      //
   }
   registerOnTouched(fn: any): void {
      //
   }
}
