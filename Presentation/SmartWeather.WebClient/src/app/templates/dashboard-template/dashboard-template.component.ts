import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DashboardHeaderComponent } from '@components/organisms/templates/dashboard-header/dashboard-header.component';
import { DashboardSidebarComponent } from '@components/organisms/templates/dashboard-sidebar/dashboard-sidebar.component';
import { AuthService } from '@services/core/auth.service';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-dashboard-template',
  imports: [DashboardSidebarComponent, DashboardHeaderComponent],
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
