import { Component, Input } from '@angular/core';
import { UserQuickAccessComponent } from '@components/molecules/user-quick-access/user-quick-access.component';

@Component({
  selector: 'app-dashboard-header',
  imports: [UserQuickAccessComponent],
  templateUrl: './dashboard-header.component.html',
  styleUrl: './dashboard-header.component.css'
})
export class DashboardHeaderComponent {

  getCurrentPageTitle(): string {
    return 'Dashboard';
  }
}
