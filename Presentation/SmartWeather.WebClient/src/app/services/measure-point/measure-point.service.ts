import { Injectable } from '@angular/core';
import { ApiResponse } from '@models/api-response';
import { MeasurePointResponse, MeasurePointUpdateRequest } from '@models/dtos/measurepoint-dtos';
import { HttpApiService } from '@services/transport/http-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MeasurePointService {

  private baseEndpoint = 'MeasurePoint';

  constructor(private httpApiService: HttpApiService) { }

  /**
   * Update a measurepoint
   */
  update(updatedMeasurePoint: MeasurePointUpdateRequest): Observable<ApiResponse<MeasurePointResponse>> {
    return this.httpApiService.put<MeasurePointResponse>(`${this.baseEndpoint}/Update?`, updatedMeasurePoint);
  }

}
