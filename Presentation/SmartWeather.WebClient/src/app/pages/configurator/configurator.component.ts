import { Component } from '@angular/core';
import { ConfiguratorSelectorComponent } from '@components/organisms/configurator-selector/configurator-selector.component';
import { ConfiguratorStationComponent } from '@components/organisms/configurator-station/configurator-station.component';
import { StationResponse } from '@models/dtos/station-dtos';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-configurator',
  imports: [DashboardTemplateComponent, ConfiguratorSelectorComponent, ConfiguratorStationComponent],
  templateUrl: './configurator.component.html',
  styleUrl: './configurator.component.css'
})
export class ConfiguratorPageComponent {

  currentSelectedStation: StationResponse | null = null;

  handleStationSectedEvent(stationSelcted: StationResponse) {
    this.currentSelectedStation = stationSelcted;
  }

}
