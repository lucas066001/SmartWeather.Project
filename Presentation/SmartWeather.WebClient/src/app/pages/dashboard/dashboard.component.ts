import { AfterViewInit, Component, OnInit } from '@angular/core';
import { CommonChartComponent } from '@components/atoms/common-chart/common-chart.component';
import { DateStationFilterBarComponent } from '@components/molecules/date-station-filter-bar/date-station-filter-bar.component';
import { MappedStationsListComponent } from '@components/organisms/stations/mapped-stations-list/mapped-stations-list.component';
import { Status } from '@constants/api/api-status';
import { MeasureUnit } from '@constants/entities/measure-unit';
import { ApiResponse } from '@models/api-response';
import { ComponentListResponse } from '@models/dtos/component-dtos';
import { StationResponse } from '@models/dtos/station-dtos';
import { TimeSerie } from '@models/ui/charting';
import { ComponentService } from '@services/component/component.service';
import { AuthService } from '@services/core/auth.service';
import { MeasureDataService } from '@services/measure-data/measure-data.service';
import { StationService } from '@services/station/station.service';
import { DashboardTemplateComponent } from '@templates/dashboard-template/dashboard-template.component';
import { connect, getInstanceByDom } from 'echarts/core';

@Component({
  selector: 'app-dashboard',
  imports: [DashboardTemplateComponent, MappedStationsListComponent, DateStationFilterBarComponent, CommonChartComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardPageComponent implements OnInit, AfterViewInit {

  stationsFromUser: StationResponse[] = [];
  temperatureSeries: TimeSerie[] = [
    { name: 'Série 1', color: "#000000", data: [{ x: new Date(1), y: 10 }, { x: new Date(2), y: 20 }, { x: new Date(3), y: 15 }] },
    { name: 'Série 2', color: "#000000", data: [{ x: new Date(1), y: 5 }, { x: new Date(2), y: 15 }, { x: new Date(3), y: 25 }] }
  ];
  humiditySeries = [
    { name: 'Série 1', color: "#000000", data: [{ x: new Date(1), y: 10 }, { x: new Date(2), y: 20 }, { x: new Date(3), y: 15 }] },
    { name: 'Série 2', color: "#000000", data: [{ x: new Date(1), y: 5 }, { x: new Date(2), y: 15 }, { x: new Date(3), y: 25 }] }
  ];


  constructor(private authService: AuthService, private stationService: StationService, private componentService: ComponentService, private measureDataService: MeasureDataService) { }

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
            let tmpTemperatureSeries: TimeSerie[] = [];
            let tmpHumiditySeries: TimeSerie[] = [];
            response.data.componentList?.forEach(component => {
              component.measurePoints?.forEach(measurePoint => {
                switch (measurePoint.unit) {
                  case MeasureUnit.Celsius:
                  case MeasureUnit.Percentage:
                    this.measureDataService.getFromMeasurePoint(measurePoint.id, filters.start, filters.end).subscribe(({
                      next: (response) => {
                        if (response.status == Status.OK && response.data) {
                          if (measurePoint.unit == MeasureUnit.Celsius) {
                            tmpTemperatureSeries.push({
                              name: measurePoint.name,
                              data: response.data.measureDataList?.map(md => ({ x: new Date(md.dateTime), y: md.value })),
                              color: measurePoint.color
                            })
                          } else {
                            tmpHumiditySeries.push({
                              name: measurePoint.name,
                              data: response.data.measureDataList?.map(md => ({ x: new Date(md.dateTime), y: md.value })),
                              color: measurePoint.color
                            })
                          }
                        }
                      },
                      error: (error: ApiResponse<ComponentListResponse>) => {

                      }
                    })
                    );
                    break;
                  default:
                    break;
                }
              });
            });
            console.log(tmpHumiditySeries);
            this.temperatureSeries = tmpTemperatureSeries;
            this.humiditySeries = tmpHumiditySeries;
          }
        },
        error: (error: ApiResponse<ComponentListResponse>) => {
          console.log('Erreur while fetching component from station' + station.name + ' :', error);
        }
      }));
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      const chartElement1 = document.getElementById('temperature-chart');
      const chartElement2 = document.getElementById('humidity-chart');
      if (chartElement1 &&
        chartElement2) {
        const chart1 = getInstanceByDom(chartElement1);
        const chart2 = getInstanceByDom(chartElement2);
        if (chart1 &&
          chart2) {
          console.log("should be connected")
          connect([chart1, chart2]);
        }
      }
    });
  }
}
