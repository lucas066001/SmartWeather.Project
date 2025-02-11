import { CommonModule } from '@angular/common';
import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-input-text',
  templateUrl: './input-text.component.html',
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputTextComponent),
      multi: true,
    },
  ],
})
export class InputTextComponent implements ControlValueAccessor {
  @Input() type: string = 'text';
  @Input() iconName?: string;
  @Input() label?: string;
  @Input() placeholder: string = '';
  @Input() isError: boolean = false;
  @Input() errorMessage: string = '';
  @Input() disabled: boolean = false;

  inputId: string = `input-${crypto.randomUUID()}`;
  availableTypes: string[] = ['text', 'password', 'email', 'phone'];
  private innerValue: any = '';

  constructor(public themeService: ThemeService) { }

  // Angular appelle cette fonction pour récupérer la valeur actuelle
  get value(): any {
    return this.innerValue;
  }

  // Angular appelle cette fonction pour mettre à jour la valeur
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChange(v);
    }
  }

  onChange: any = () => { };
  onTouched: any = () => { };

  // Méthodes requises par ControlValueAccessor
  writeValue(value: any): void {
    this.innerValue = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
