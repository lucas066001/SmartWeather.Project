import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-switch',
  imports: [FormsModule, CommonModule],
  templateUrl: './switch.component.html',
  styleUrl: './switch.component.css'
})
export class SwitchComponent {
  @Input() checkedText: string = 'Checked';
  @Input() uncheckedText: string = 'Unchecked';
  @Input() theme: string = 'green';
  @Input() showLabel: boolean = true;
  @Input() intialState: boolean = false;

  @Output() checkEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  availableThemes: string[] = ['green', 'brown'];

  isChecked: boolean = false;

  constructor() { }

  ngOnInit() {
    this.isChecked = this.intialState;
  }

  clickTrigger() {
    this.isChecked = !this.isChecked;
    this.checkEvent.emit(this.isChecked);
  }
}
