import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { DateSelectorComponent } from '@components/atoms/date-selector/date-selector.component';
import { StationDropdownComponent } from '../station-dropdown/station-dropdown.component';
import { StationResponse } from '@models/dtos/station-dtos';

@Component({
  selector: 'app-date-station-filter-bar',
  imports: [DateSelectorComponent, ButtonComponent, StationDropdownComponent],
  templateUrl: './date-station-filter-bar.component.html',
  styleUrl: './date-station-filter-bar.component.css'
})
export class DateStationFilterBarComponent {
  @Input() stationsInFilter: StationResponse[] = [];
  @Output() filterApplyEvent: EventEmitter<{ start: Date, end: Date, stations: StationResponse[] }> = new EventEmitter();

  private stationsSelected: StationResponse[] = [];
  private startDate: Date | null = null;
  private endDate: Date | null = null;

  handleStartDateChange(dateReceived: string | null) {
    if (dateReceived) {
      this.startDate = new Date(dateReceived);
    }
  }

  handleEndDateChange(dateReceived: string | null) {
    if (dateReceived) {
      this.endDate = new Date(dateReceived);
    }
  }

  handleStationListChange(stationsReceived: StationResponse[]) {
    this.stationsSelected = stationsReceived;
  }

  handleApplyChanges() {
    if (this.stationsSelected.length > 0 &&
      this.startDate &&
      this.endDate
    ) {
      this.filterApplyEvent.emit({ start: this.startDate, end: this.endDate, stations: this.stationsSelected });
      console.log("Filtres : ", this.stationsSelected, this.startDate, this.endDate)
    }
  }
}
