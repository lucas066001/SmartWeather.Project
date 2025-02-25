import { Component, Input } from '@angular/core';
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

}
