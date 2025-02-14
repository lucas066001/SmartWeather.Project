import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
export class ConfiguratorSelectorComponent {

  @Output() stationSelectedEvent: EventEmitter<StationResponse> = new EventEmitter<StationResponse>();

  @Input() stationsFromUser: StationResponse[] = [];

  handleNewStationClaimed(newStation: StationResponse) {
    this.stationsFromUser.push(newStation);
  }

  handleStationSelected(selectedStation: StationResponse) {
    this.stationSelectedEvent.emit(selectedStation);
  }
}
