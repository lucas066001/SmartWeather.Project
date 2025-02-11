import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@services/core/auth.service';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-dashboard-template',
  imports: [],
  templateUrl: './dashboard-template.component.html',
  styleUrl: './dashboard-template.component.css'
})
export class DashboardTemplateComponent {
  constructor(public themeService: ThemeService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
    }
  }
}
