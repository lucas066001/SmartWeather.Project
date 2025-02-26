import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StationClaimToolComponent } from '@components/molecules/station-claim-tool/station-claim-tool.component';
import { StationListComponent } from '@components/molecules/station-list/station-list.component';
import { StationResponse } from '@models/dtos/station-dtos';

@Component({
  selector: 'app-configurator-selector',
  imports: [StationClaimToolComponent, StationListComponent],
  templateUrl: './configurator-selector.component.html',
  styleUrl: './configurator-selector.component.css'
})
export class ConfiguratorSelectorComponent {

  @Output() stationSelectedEvent: EventEmitter<number> = new EventEmitter<number>();

  @Input() stationsFromUser: StationResponse[] = [];

  handleNewStationClaimed(newStation: StationResponse) {
    this.stationsFromUser.push(newStation);
  }

  handleStationSelected(selectedStation: StationResponse) {
    this.stationSelectedEvent.emit(selectedStation.id);
  }
}
