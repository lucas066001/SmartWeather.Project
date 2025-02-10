import { Injectable } from '@angular/core';
import { UITheme } from '@constants/ui-theme';

@Injectable({
  providedIn: 'root'
})

export class ThemeService {

  private readonly storageKey = 'sm-theme';

  constructor() {
    this.loadTheme();
  }

  setTheme(theme: UITheme) {
    document.body.className = theme;
    localStorage.setItem(this.storageKey, theme);
  }

  private loadTheme() {
    const savedTheme = localStorage.getItem(this.storageKey) || UITheme.LIGHT;
    document.body.className = savedTheme;
  }
}
