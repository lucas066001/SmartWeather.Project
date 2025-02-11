import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpApiService } from '@services/transport/http-api.service';
import { ApiResponse } from '@models/api-response';
import { UserRegisterRequest, UserSigninRequest, UserRegisterResponse, UserSigninResponse } from '@models/dtos/authentication-dtos'; // Adjust the path to your models

enum AuthenticationEndpoints {
  REGISTER = 'Register',
  SIGNIN = 'Signin'
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private baseEndpoint = 'Authentication';  // Base endpoint for authentication

  constructor(private httpApiService: HttpApiService) { }

  /**
   * Register a new user
   * @param user The registration request data
   * @returns Observable<ApiResponse<UserRegisterResponse>>
   */
  register(user: UserRegisterRequest): Observable<ApiResponse<UserRegisterResponse>> {
    return this.httpApiService.post<UserRegisterResponse>(
      `${this.baseEndpoint}/${AuthenticationEndpoints.REGISTER}`,
      user
    );
  }

  /**
   * Sign in an existing user
   * @param user The sign-in request data
   * @returns Observable<ApiResponse<UserSigninResponse>>
   */
  signin(user: UserSigninRequest): Observable<ApiResponse<UserSigninResponse>> {
    return this.httpApiService.post<UserSigninResponse>(
      `${this.baseEndpoint}/${AuthenticationEndpoints.SIGNIN}`,
      user
    );
  }
}
