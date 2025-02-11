import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from '@constants/roles';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private tokenKey = 'auth-token';
  private jwtHelper = new JwtHelperService();

  constructor() { }

  /**
   * Stores the JWT token in localStorage
   * @param token JWT Token
   */
  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  /**
   * Retrieves the JWT token from localStorage
   * @returns JWT token or null
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Removes the token from localStorage (Logout)
   */
  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  /**
   * Checks if the user is authenticated (valid and non-expired token)
   * @returns true if authenticated, false otherwise
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }

  /**
   * Checks if the token is expired
   * @returns true if expired, false otherwise
   */
  isTokenExpired(): boolean {
    const token = this.getToken();
    return token ? this.jwtHelper.isTokenExpired(token) : true;
  }

  /**
   * Retrieves the user's email from the token
   * @returns Email or null
   */
  getUserEmail(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken?.email || null;
  }

  /**
   * Retrieves the user's role from the token as an enum
   * @returns Role (Admin, User, Unauthorized) or Unauthorized by default
   */
  getUserRole(): Role {
    const token = this.getToken();
    if (!token) return Role.Unauthorized;

    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken?.role !== undefined ? Number(decodedToken.role) as Role : Role.Unauthorized;
  }

  /**
   * Retrieves the user's ID from the token as a number
   * @returns User ID (number) or null if not found
   */
  getUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;

    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken?.nameid !== undefined ? Number(decodedToken.nameid) : null;
  }
}
