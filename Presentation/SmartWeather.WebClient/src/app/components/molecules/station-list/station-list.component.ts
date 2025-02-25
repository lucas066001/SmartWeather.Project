import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { EditableListElementComponent } from '@components/atoms/editable-list-element/editable-list-element.component';
import { StationType } from '@constants/entities/station-type';
import { StationResponse } from '@models/dtos/station-dtos';

@Component({
  selector: 'app-station-list',
  imports: [CommonModule, EditableListElementComponent],
  templateUrl: './station-list.component.html',
  styleUrl: './station-list.component.css'
})
export class StationListComponent {

  @Input() stations: StationResponse[] = [];
  @Output() stationSelectEvent: EventEmitter<StationResponse> = new EventEmitter<StationResponse>();
  @Output() stationEditEvent: EventEmitter<StationResponse> = new EventEmitter<StationResponse>();

  selectedStationId: number | null = null;


  handleStationSelect(stationId: number) {
    let foundStation = this.stations.find(station => station.id === stationId);
    this.stationSelectEvent.emit(foundStation);
    this.selectedStationId = stationId;
    console.log(`Station ${foundStation} selected`);
  }

  handleStationEdit(stationId: number) {
    let foundStation = this.stations.find(station => station.id === stationId);
    this.stationEditEvent.emit(foundStation);
    this.selectedStationId = stationId;
    console.log(`Station ${foundStation} edited`);
  }

}
