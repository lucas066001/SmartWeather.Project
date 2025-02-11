import { Component } from '@angular/core';
import { SwitchComponent } from '@components/atoms/switch/switch.component';
import { UITheme } from '@constants/ui-theme';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-theme-selector',
  imports: [SwitchComponent],
  templateUrl: './theme-selector.component.html',
  styleUrl: './theme-selector.component.css'
})
export class ThemeSelectorComponent {
  isDarkTheme: boolean = false;

  constructor(public themeService: ThemeService) {
    this.isDarkTheme = this.themeService.getTheme() == UITheme.DARK;
  }

  toggleTheme(isDarkTheme: boolean) {
    const newTheme = isDarkTheme ? UITheme.DARK : UITheme.LIGHT;
    this.themeService.setTheme(newTheme);
  }
}
