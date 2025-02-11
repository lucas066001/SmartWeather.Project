import { Injectable } from '@angular/core';
import { UITheme } from '@constants/ui-theme';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly storageKey = 'sm-theme';
  private themeSubject = new BehaviorSubject<UITheme>(UITheme.LIGHT);
  theme = this.themeSubject.asObservable();

  constructor() {
    this.loadTheme();
  }

  /**
   * Sets the theme and updates the body class & local storage.
   * @param theme The theme to apply (light or dark).
   */
  setTheme(theme: UITheme) {
    document.body.className = theme;
    localStorage.setItem(this.storageKey, theme);
    this.themeSubject.next(theme);
  }

  /**
 * Gets the theme from  local storage.
 * @param theme The theme to apply (light or dark).
 * @returns The current theme.
 */
  getTheme() {
    return (localStorage.getItem(this.storageKey) as UITheme) || UITheme.LIGHT;
  }

  /**
   * Loads the theme from local storage and applies it.
   */
  private loadTheme() {
    const savedTheme = (localStorage.getItem(this.storageKey) as UITheme) || UITheme.LIGHT;
    document.body.className = savedTheme;
    this.themeSubject.next(savedTheme);
  }

  /**
   * Returns the correct icon path based on the current theme.
   * @param iconName The name of the icon (without extension).
   * @returns The path to the corresponding theme-based icon.
   */
  getIconPath(iconName: string): string {
    const theme = this.themeSubject.value;
    return `/assets/icons/${theme === UITheme.DARK ? 'dark' : 'light'}/${iconName}.svg`;
  }
}
