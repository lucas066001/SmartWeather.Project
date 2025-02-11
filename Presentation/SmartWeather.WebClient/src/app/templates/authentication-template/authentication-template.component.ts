import { Component, Input, Signal, signal } from '@angular/core';
import { AuthenticationService } from '@services/authentication/authentication.service';
import { ThemeSelectorComponent } from '@components/molecules/theme-selector/theme-selector.component';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-authentication-template',
  imports: [ThemeSelectorComponent],
  templateUrl: './authentication-template.component.html',
  styleUrl: './authentication-template.component.css'
})
export class AuthenticationTemplateComponent {
  @Input() title: string = '';

  constructor(public themeService: ThemeService, private authService: AuthenticationService) { }

  loginWithGoogle() {
    console.log('loginWithGoogle');
    // this.authService.loginWithGoogle();
  }
}
