import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-home-icon',
  imports: [CommonModule],
  templateUrl: './home-icon.component.html',
  styleUrl: './home-icon.component.css'
})
export class HomeIconComponent {
  @Input() isSelected: boolean = false;
}
