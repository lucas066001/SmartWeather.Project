import { Injectable } from '@angular/core';
import { UITheme } from '@constants/ui/ui-theme';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly currentThemeKey = 'sm-theme';
  private readonly currentLayoutStateKey = 'sm-layout-state';
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
    localStorage.setItem(this.currentThemeKey, theme);
    this.themeSubject.next(theme);
  }

  /**
 * Gets the theme from  local storage.
 * @returns The current theme.
 */
  getTheme(): UITheme {
    return (localStorage.getItem(this.currentThemeKey) as UITheme) || UITheme.LIGHT;
  }

  /**
 * Sets the current layout theme and updates the local storage.
 * @param expanded The new state (true or false).
 */
  setLayoutState(theme: boolean) {
    localStorage.setItem(this.currentLayoutStateKey, theme.valueOf().toString());
  }

  /**
 * Gets the the current layout state from  local storage.
 * @returns The current state.
 */
  getLayoutState(): boolean {
    return (localStorage.getItem(this.currentLayoutStateKey)) === 'true';
  }

  /**
   * Loads the theme from local storage and applies it.
   */
  private loadTheme() {
    const savedTheme = (localStorage.getItem(this.currentThemeKey) as UITheme) || UITheme.LIGHT;
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
