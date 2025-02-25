import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { StationResponse } from '@models/dtos/station-dtos';

@Component({
  selector: 'app-station-dropdown',
  imports: [CommonModule],
  templateUrl: './station-dropdown.component.html',
  styleUrl: './station-dropdown.component.css'
})
export class StationDropdownComponent {
  @Input() stations: StationResponse[] = [];
  @Output() selectionChange: EventEmitter<StationResponse[]> = new EventEmitter<StationResponse[]>();

  isOpen = false;
  selectedStations: StationResponse[] = [];

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }

  toggleSelection(station: StationResponse) {
    const index = this.selectedStations.findIndex((s) => s.id === station.id);
    if (index === -1) {
      this.selectedStations.push(station);
    } else {
      this.selectedStations.splice(index, 1);
    }
    this.emitSelection();
  }

  toggleSelectAll() {
    if (this.selectedStations.length === this.stations.length) {
      this.selectedStations = [];
    } else {
      this.selectedStations = [...this.stations];
    }
    this.emitSelection();
  }

  emitSelection() {
    this.selectionChange.emit(this.selectedStations);
  }

  isSelected(station: StationResponse): boolean {
    return this.selectedStations.some((s) => s.id === station.id);
  }
}
