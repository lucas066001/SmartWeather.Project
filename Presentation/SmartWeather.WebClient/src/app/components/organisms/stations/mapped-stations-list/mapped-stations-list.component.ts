import { afterNextRender, AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MapPointsComponent } from '@components/atoms/map-points/map-points.component';
import { StationListComponent } from '@components/molecules/station-list/station-list.component';
import { StationResponse } from '@models/dtos/station-dtos';
import { LatLngExpression } from 'leaflet';

@Component({
  selector: 'app-mapped-stations-list',
  imports: [MapPointsComponent, StationListComponent],
  templateUrl: './mapped-stations-list.component.html',
  styleUrl: './mapped-stations-list.component.css'
})
export class MappedStationsListComponent {
  @Input()
  set stationsFromUser(value: StationResponse[]) {
    this._stationsFromUser = value;
    setTimeout(() => {
      this.generateCoorList();
    }, 100);
  }
  get stationsFromUser(): StationResponse[] {
    return this._stationsFromUser;
  }

  stationsCoordinates: LatLngExpression[] = [];
  currentSelectedCoordinates: LatLngExpression | null = null;

  private _stationsFromUser: StationResponse[] = [];

  constructor(private router: Router) {
    afterNextRender({
      write: () => {
        this.generateCoorList();
      }
    });
  }

  handleStationSelected(selectedStation: StationResponse) {
    this.currentSelectedCoordinates = [selectedStation.latitude, selectedStation.longitude];
  }

  handleStationEdited(selectedStation: StationResponse) {
    this.router.navigate(["/config/", selectedStation.id]);
  }

  generateCoorList() {
    this.stationsCoordinates = this._stationsFromUser.map(station => [station.latitude, station.longitude]);
  }
}
