import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@services/core/auth.service';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-user-quick-access',
  imports: [CommonModule],
  templateUrl: './user-quick-access.component.html',
  styleUrl: './user-quick-access.component.css'
})
export class UserQuickAccessComponent {

  isMenuExpanded: boolean = false;

  constructor(public themeService: ThemeService, private authService: AuthService, private router: Router) { }

  toggleMenu() {
    this.isMenuExpanded = !this.isMenuExpanded;
  }

  diconnect() {
    this.authService.clearToken();
    this.router.navigate(['/login']);
  }

}
