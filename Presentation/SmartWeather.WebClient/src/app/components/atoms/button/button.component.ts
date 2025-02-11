import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {
  @Input() theme: string = 'green';
  @Input() iconName: string | null = null;
  @Input() type: string = "button";
  @Input() disabled: boolean = false;

  availableThemes: string[] = ['green', 'brown', 'red'];

  constructor(public themeService: ThemeService) { }

  getButtonClasses(): string {
    const themeColors: { [key: string]: string } = {
      green: 'bg-[var(--sm-primary-color)] hover:bg-[var(--sm-primary-color)]/90 focus:ring-[var(--sm-primary-color)]/50 dark:focus:ring-[var(--sm-primary-color)]/55 ',
      red: 'bg-[var(--sm-error-color)] hover:bg-[var(--sm-error-color)]/90 focus:ring-[var(--sm-error-color)]/50 dark:focus:ring-[var(--sm-error-color)]/55 '
    };
    let retreivedClasses: string = this.availableThemes.includes(this.theme) ? themeColors[this.theme] : themeColors['green'];
    this.disabled ?
      retreivedClasses += 'cursor-not-allowed opacity-50' :
      retreivedClasses += 'cursor-pointer';
    return retreivedClasses;
  }


}
