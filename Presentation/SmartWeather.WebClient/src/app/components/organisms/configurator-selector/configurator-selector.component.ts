import { Component, OnInit } from '@angular/core';
import { StationClaimToolComponent } from '@components/molecules/station-claim-tool/station-claim-tool.component';
import { StationListComponent } from '@components/molecules/station-list/station-list.component';
import { Status } from '@constants/api-status';
import { StationResponse } from '@models/dtos/station-dtos';
import { AuthService } from '@services/core/auth.service';
import { StationService } from '@services/station/station.service';

@Component({
  selector: 'app-configurator-selector',
  imports: [StationClaimToolComponent, StationListComponent],
  templateUrl: './configurator-selector.component.html',
  styleUrl: './configurator-selector.component.css'
})
export class ConfiguratorSelectorComponent implements OnInit {

  stationsFromUser: StationResponse[] = [];

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

  handleNewStationClaimed(newStation: StationResponse) {
    this.stationsFromUser.push(newStation);
  }
}
