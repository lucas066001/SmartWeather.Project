import { Component } from '@angular/core';
import { ConfiguratorSelectorComponent } from '@components/organisms/configurator-selector/configurator-selector.component';
import { ConfiguratorStationComponent } from '@components/organisms/configurator-station/configurator-station.component';
import { Status } from '@constants/api/api-status';
import { StationResponse } from '@models/dtos/station-dtos';
import { AuthService } from '@services/core/auth.service';
import { StationService } from '@services/station/station.service';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-configurator',
  imports: [DashboardTemplateComponent, ConfiguratorSelectorComponent, ConfiguratorStationComponent],
  templateUrl: './configurator.component.html',
  styleUrl: './configurator.component.css'
})
export class ConfiguratorPageComponent {

  stationsFromUser: StationResponse[] = [];
  currentSelectedStation: StationResponse | null = null;

  constructor(private stationService: StationService, private authService: AuthService) { }

  ngOnInit(): void {
    let retreivedUserId = this.authService.getUserId();
    if (retreivedUserId) {

      this.stationService.getFromUser(retreivedUserId).subscribe({
        next: (response) => {
          console.log('Stations retreived :', response);
          if (response.status == Status.OK && response.data?.stationList && response.data.stationList.length > 0) {
            this.stationsFromUser.push(...response.data.stationList);
            console.log(this.stationsFromUser);
          }
        },
        error: (error) => {
          console.error('Error while retreiving stations :', error);
        }
      });
    }
  }

  handleStationSectedEvent(stationSelcted: StationResponse) {
    this.currentSelectedStation = stationSelcted;
  }

  transmitUpdatedStationInfosToList(updatedStation: StationResponse) {
    let modifiedStationIndex = this.stationsFromUser.findIndex(s => s.id == updatedStation.id);
    if (modifiedStationIndex >= 0) {
      this.stationsFromUser[modifiedStationIndex] = updatedStation;
    }
  }

}
