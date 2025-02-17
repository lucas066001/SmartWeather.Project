import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-editable-list-element',
  imports: [CommonModule],
  templateUrl: './editable-list-element.component.html',
  styleUrl: './editable-list-element.component.css'
})
export class EditableListElementComponent {

  @Input() text: string = '';
  @Input() theme: string = 'green';
  @Input() isSelected: boolean = false;
  @Input() itemId: number = -1;

  @Output() selectEvent: EventEmitter<number> = new EventEmitter<number>();

  availableThemes: string[] = ['green', 'blue'];

  constructor(public themeService: ThemeService) { }

  triggerSelectEvent() {
    this.selectEvent.emit(this.itemId);
  }

}
