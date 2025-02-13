import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { UserQuickAccessComponent } from '@components/molecules/user-quick-access/user-quick-access.component';
import { AppPages } from '@constants/app-pages';

@Component({
  selector: 'app-dashboard-header',
  imports: [UserQuickAccessComponent],
  templateUrl: './dashboard-header.component.html',
  styleUrl: './dashboard-header.component.css'
})
export class DashboardHeaderComponent {

  constructor(private router: Router) { }

  getCurrentPageTitle(): string {
    const currentUrl = this.router.url;
    const matchedKey = Array.from(AppPages.keys()).find(key => currentUrl.startsWith(key));
    return matchedKey ? AppPages.get(matchedKey)! : 'Unknown';
  }
}
