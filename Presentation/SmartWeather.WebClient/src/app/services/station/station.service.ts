import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpApiService } from '@services/transport/http-api.service';
import { ApiResponse } from '@models/api-response';
import { StationResponse, StationClaimRequest, StationListResponse, StationUpdateRequest } from '@models/dtos/station-dtos';

@Injectable({
  providedIn: 'root'
})
export class StationService {

  private baseEndpoint = 'Station';

  constructor(private httpApiService: HttpApiService) { }

  // /**
  //  * Create a new station
  //  */
  // create(request: StationCreateRequest): Observable<ApiResponse<StationResponse>> {
  //   return this.httpApiService.post<StationResponse>(`${this.baseEndpoint}/Create`, request);
  // }

  /**
   * Claim a station
   */
  claim(request: StationClaimRequest): Observable<ApiResponse<StationResponse>> {
    return this.httpApiService.post<StationResponse>(`${this.baseEndpoint}/Claim`, request);
  }

  /**
   * Update a station
   */
  update(request: StationUpdateRequest): Observable<ApiResponse<StationResponse>> {
    return this.httpApiService.put<StationResponse>(`${this.baseEndpoint}/Update`, request);
  }

  // /**
  //  * Delete a station by ID
  //  */
  // delete(idStation: number): Observable<ApiResponse<void>> {
  //   return this.httpApiService.delete<void>(`${this.baseEndpoint}/Delete?idStation=${idStation}`);
  // }

  /**
   * Get stations owned by a user
   */
  getFromUser(userId: number): Observable<ApiResponse<StationListResponse>> {
    return this.httpApiService.get<StationListResponse>(`${this.baseEndpoint}/GetFromUser?userId=${userId}`);
  }

  /**
   * Get a station by its ID
   */
  getById(idStation: number): Observable<ApiResponse<StationResponse>> {
    return this.httpApiService.get<StationResponse>(`${this.baseEndpoint}/GetById?idStation=${idStation}`);
  }

  // /**
  //  * Get all stations with optional parameters
  //  */
  // getAll(includeComponents: boolean, includeMeasurePoints: boolean): Observable<ApiResponse<StationListResponse>> {
  //   return this.httpApiService.get<StationListResponse>(`${this.baseEndpoint}GetAll?includeComponents=${includeComponents}&includeMeasurePoints=${includeMeasurePoints}`);
  // }

  // /**
  //  * Get measurement points of all stations
  //  */
  // getStationsMeasurePoints(): Observable<ApiResponse<StationMeasurePointsResponse[]>> {
  //   return this.httpApiService.get<StationMeasurePointsResponse[]>(`${this.baseEndpoint}/GetStationsMeasurePoints`);
  // }
}
