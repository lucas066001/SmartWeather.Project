import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dashboard-header',
  imports: [],
  templateUrl: './dashboard-header.component.html',
  styleUrl: './dashboard-header.component.css'
})
export class DashboardHeaderComponent {

  getCurrentPageTitle(): string {
    return 'Dashboard';
  }
}
