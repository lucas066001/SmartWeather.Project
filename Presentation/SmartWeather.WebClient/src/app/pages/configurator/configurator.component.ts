import { Component } from '@angular/core';
import { ConfiguratorSelectorComponent } from '@components/organisms/configurator-selector/configurator-selector.component';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-configurator',
  imports: [DashboardTemplateComponent, ConfiguratorSelectorComponent],
  templateUrl: './configurator.component.html',
  styleUrl: './configurator.component.css'
})
export class ConfiguratorPageComponent {

}
