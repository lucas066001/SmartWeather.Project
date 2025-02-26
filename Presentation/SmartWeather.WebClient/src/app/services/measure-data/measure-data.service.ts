import { Injectable } from '@angular/core';
import { ApiResponse } from '@models/api-response';
import { MeasureDataListResponse } from '@models/dtos/measure-data-dtos';
import { HttpApiService } from '@services/transport/http-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MeasureDataService {

  private baseEndpoint = 'MeasureData';

  constructor(private httpApiService: HttpApiService) { }

  /**
   * Get measure datas from measure point between two dates
   */
  getFromMeasurePoint(measurePointId: number, startPeriod: Date, endPeriod: Date): Observable<ApiResponse<MeasureDataListResponse>> {
    return this.httpApiService.get<MeasureDataListResponse>(`${this.baseEndpoint}/GetFromMeasurePoint?`, { 'measurePointId': measurePointId, 'startPeriod': startPeriod.toUTCString(), 'endPeriod': endPeriod.toUTCString() });
  }

}
