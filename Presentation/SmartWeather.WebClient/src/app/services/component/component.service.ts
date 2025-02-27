import { Injectable } from '@angular/core';
import { ApiResponse } from '@models/api-response';
import { ActuatorCommandRequest, ComponentListResponse, ComponentResponse, ComponentUpdateRequest } from '@models/dtos/component-dtos';
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

  /**
   * Update a component
   */
  update(updatedComponent: ComponentUpdateRequest): Observable<ApiResponse<ComponentResponse>> {
    return this.httpApiService.put<ComponentResponse>(`${this.baseEndpoint}/Update`, updatedComponent);
  }

  actuatorCommand(actuatorRequest: ActuatorCommandRequest): Observable<ApiResponse<undefined>> {
    return this.httpApiService.put<undefined>(`${this.baseEndpoint}/ActuatorCommand`, actuatorRequest);
  }

}
