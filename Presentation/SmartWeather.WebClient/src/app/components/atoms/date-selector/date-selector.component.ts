import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-date-selector',
  imports: [],
  templateUrl: './date-selector.component.html',
  styleUrl: './date-selector.component.css'
})
export class DateSelectorComponent {
  @Input() initialDate: string | null = null;
  @Output() dateChange: EventEmitter<string | null> = new EventEmitter<string | null>();

  currentDate: string | null = null;
  isDisabled = false;

  onChange: (value: string | null) => void = () => { };
  onTouched: () => void = () => { };

  ngOnInit() {
    this.currentDate = this.initialDate;
  }

  onDateChange(event: Event) {
    const value = (event.target as HTMLInputElement).value || null;
    this.currentDate = value;
    this.dateChange.emit(this.currentDate);
    console.log(this.currentDate);
    this.onChange(this.currentDate);
    this.onTouched();
  }

  writeValue(value: string | null): void {
    this.currentDate = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }
}
