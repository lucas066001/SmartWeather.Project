import { CommonModule } from '@angular/common';
import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-checkbox',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxComponent),
      multi: true,
    },
  ],
})
export class CheckboxComponent implements ControlValueAccessor {
  @Input() label: string | null = null;
  @Input() disabled: boolean = false;

  isChecked: boolean = false;

  constructor() { }

  // Méthodes requises par ControlValueAccessor
  onChange = (value: boolean) => { };
  onTouched = () => { };

  writeValue(value: boolean): void {
    this.isChecked = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  toggleCheckbox(): void {
    if (!this.disabled) {
      this.isChecked = !this.isChecked;
      this.onChange(this.isChecked);
      this.onTouched();
    }
  }
}
