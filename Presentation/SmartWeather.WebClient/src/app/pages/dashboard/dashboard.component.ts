import { Component, OnInit } from '@angular/core';
import { MappedStationsListComponent } from '@components/organisms/stations/mapped-stations-list/mapped-stations-list.component';
import { Status } from '@constants/api/api-status';
import { StationResponse } from '@models/dtos/station-dtos';
import { AuthService } from '@services/core/auth.service';
import { StationService } from '@services/station/station.service';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardTemplateComponent, MappedStationsListComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardPageComponent implements OnInit {

  stationsFromUser: StationResponse[] = [];

  constructor(private authService: AuthService, private stationService: StationService) { }

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

}
