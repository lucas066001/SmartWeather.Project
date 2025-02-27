import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-water-can-icon',
  imports: [CommonModule],
  templateUrl: './water-can-icon.component.html',
  styleUrl: './water-can-icon.component.css'
})
export class WaterCanIconComponent {
  @Input() isSelected: boolean = false;
}
