import { Component } from '@angular/core';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardTemplateComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardPageComponent {

}
