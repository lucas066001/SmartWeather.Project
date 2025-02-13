import { Component, Input, OnInit } from '@angular/core';
import { ThemeSelectorComponent } from '@components/molecules/theme-selector/theme-selector.component';
import { ThemeService } from '@services/core/theme.service';
import { AuthService } from '@services/core/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication-template',
  imports: [ThemeSelectorComponent],
  templateUrl: './authentication-template.component.html',
  styleUrl: './authentication-template.component.css'
})
export class AuthenticationTemplateComponent implements OnInit {
  @Input() title: string = '';

  constructor(public themeService: ThemeService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

}
