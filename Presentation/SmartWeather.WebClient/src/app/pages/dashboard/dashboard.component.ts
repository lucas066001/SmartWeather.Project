import { Component, OnInit } from '@angular/core';
import { consumerMarkDirty } from '@angular/core/primitives/signals';
import { DateStationFilterBarComponent } from '@components/molecules/date-station-filter-bar/date-station-filter-bar.component';
import { MappedStationsListComponent } from '@components/organisms/stations/mapped-stations-list/mapped-stations-list.component';
import { Status } from '@constants/api/api-status';
import { MeasureUnit } from '@constants/entities/measure-unit';
import { ApiResponse } from '@models/api-response';
import { ComponentListResponse } from '@models/dtos/component-dtos';
import { StationResponse } from '@models/dtos/station-dtos';
import { ComponentService } from '@services/component/component.service';
import { AuthService } from '@services/core/auth.service';
import { StationService } from '@services/station/station.service';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardTemplateComponent, MappedStationsListComponent, DateStationFilterBarComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardPageComponent implements OnInit {

  stationsFromUser: StationResponse[] = [];

  constructor(private authService: AuthService, private stationService: StationService, private componentService: ComponentService) { }

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

  handleFilterChange(filters: { start: Date, end: Date, stations: StationResponse[] }) {
    filters.stations.forEach(station => {
      this.componentService.getFromStation(station.id, true).subscribe(({
        next: (response) => {
          if (response.status == Status.OK && response.data) {
            response.data.componentList?.forEach(component => {
              component.measurePoints?.forEach(measurePoint => {
                switch (measurePoint.unit) {
                  case MeasureUnit.Celsius:
                    console.log(station.name + "contains temp measure point")
                    //Retreive data from it and include it in visualization
                    break;
                  case MeasureUnit.Percentage:
                    console.log(station.name + "contains humidity measure point")
                    //Retreive data from it and include it in visualization

                    break;
                  default:
                    break;
                }
              })
            })
          }
        },
        error: (error: ApiResponse<ComponentListResponse>) => {
          console.log('Erreur while fetching component from station' + station.name + ' :', error);
        }
      }));
    });
  }

}
