import { Component } from '@angular/core';
import { ButtonComponent } from '@components/atoms/button/button.component';
import { DateSelectorComponent } from '@components/atoms/date-selector/date-selector.component';

@Component({
  selector: 'app-date-station-filter-bar',
  imports: [DateSelectorComponent, ButtonComponent],
  templateUrl: './date-station-filter-bar.component.html',
  styleUrl: './date-station-filter-bar.component.css'
})
export class DateStationFilterBarComponent {

}
