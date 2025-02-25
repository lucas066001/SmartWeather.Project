import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { PagesLinksComponent } from '@components/molecules/pages-links/pages-links.component';
import { ThemeSelectorComponent } from '@components/molecules/theme-selector/theme-selector.component';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-dashboard-sidebar',
  imports: [CommonModule, PagesLinksComponent, ThemeSelectorComponent],
  templateUrl: './dashboard-sidebar.component.html',
  styleUrl: './dashboard-sidebar.component.css'
})
export class DashboardSidebarComponent implements OnInit {

  isExpanded: boolean = true;

  constructor(public themeService: ThemeService) { }

  ngOnInit(): void {
    this.isExpanded = this.themeService.getLayoutState();
  }

  toggleSidebar() {
    this.isExpanded = !this.isExpanded
    this.themeService.setLayoutState(this.isExpanded)
  }
}
