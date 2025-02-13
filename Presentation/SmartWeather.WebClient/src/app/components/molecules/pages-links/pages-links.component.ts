import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HomeIconComponent } from '@components/atoms/menu-icons/home-icon/home-icon.component';
import { ToolsIconComponent } from '@components/atoms/menu-icons/tools-icon/tools-icon.component';
import { ThemeService } from '@services/core/theme.service';

@Component({
  selector: 'app-pages-links',
  imports: [CommonModule, HomeIconComponent, ToolsIconComponent],
  templateUrl: './pages-links.component.html',
  styleUrl: './pages-links.component.css'
})
export class PagesLinksComponent {
  @Input() isExpanded: boolean = true;

  constructor(public themeService: ThemeService, private router: Router) { }

  redirectTo(link: string) {
    this.router.navigate([link]);
  }

  isLinkActive(url: string): boolean {
    return this.router.url === url;
  }
}
