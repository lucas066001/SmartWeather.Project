import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tools-icon',
  imports: [CommonModule],
  templateUrl: './tools-icon.component.html',
  styleUrl: './tools-icon.component.css'
})
export class ToolsIconComponent {
  @Input() isSelected: boolean = false;
}
