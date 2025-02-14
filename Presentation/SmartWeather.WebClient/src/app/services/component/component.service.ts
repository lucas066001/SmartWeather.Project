import { Injectable } from '@angular/core';
import { ApiResponse } from '@models/api-response';
import { ComponentListResponse, ComponentResponse } from '@models/dtos/component-dtos';
import { HttpApiService } from '@services/transport/http-api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ComponentService {
  private baseEndpoint = 'Component';

  constructor(private httpApiService: HttpApiService) { }

  /**
   * Get components from station
   */
  getFromStation(stationId: number, includeComponents: boolean): Observable<ApiResponse<ComponentListResponse>> {
    return this.httpApiService.get<ComponentListResponse>(`${this.baseEndpoint}/GetFromStation?`, { 'stationId': stationId, 'includeComponents': includeComponents });
  }

}
