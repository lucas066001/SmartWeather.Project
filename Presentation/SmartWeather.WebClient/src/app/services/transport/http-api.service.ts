import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '@models/api-response'; // Adjust path as needed
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpApiService {

  constructor(private http: HttpClient) { }

  /**
   * Sends a GET request to the specified endpoint with optional parameters.
   * @param endpoint API endpoint (relative or absolute URL)
   * @param params Query parameters (optional)
   * @returns Observable<ApiResponse<T>>
   */
  get<T>(endpoint: string, params?: Record<string, any>): Observable<ApiResponse<T>> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        httpParams = httpParams.set(key, params[key]);
      });
    }

    return this.http.get<ApiResponse<T>>(`${environment.apiUrl}/${endpoint}`, { params: httpParams });
  }

  /**
   * Sends a POST request with a payload to the specified endpoint.
   * @param endpoint API endpoint
   * @param body Request payload
   * @returns Observable<ApiResponse<T>>
   */
  post<T>(endpoint: string, body: any): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${environment.apiUrl}/${endpoint}`, body);
  }

  /**
   * Sends a PUT request to update a resource.
   * @param endpoint API endpoint
   * @param body Request payload
   * @returns Observable<ApiResponse<T>>
   */
  put<T>(endpoint: string, body: any): Observable<ApiResponse<T>> {
    return this.http.put<ApiResponse<T>>(`${environment.apiUrl}/${endpoint}`, body);
  }

  /**
   * Sends a DELETE request to remove a resource.
   * @param endpoint API endpoint
   * @param params Query parameters (optional)
   * @returns Observable<ApiResponse<T>>
   */
  delete<T>(endpoint: string, params?: Record<string, any>): Observable<ApiResponse<T>> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        httpParams = httpParams.set(key, params[key]);
      });
    }

    return this.http.delete<ApiResponse<T>>(`${environment.apiUrl}/${endpoint}`, { params: httpParams });
  }
}
