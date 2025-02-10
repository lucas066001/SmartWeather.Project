import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from '@services/core/theme.service';
import { UITheme } from '@constants/ui-theme';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'SmartWeather.WebClient';

  constructor(private themeService: ThemeService) { }

  setLightTheme() {
    this.themeService.setTheme(UITheme.LIGHT);
  }

  setDarkTheme() {
    this.themeService.setTheme(UITheme.DARK);
  }
}
