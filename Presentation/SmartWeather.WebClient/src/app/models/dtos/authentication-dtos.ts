export interface UserRegisterRequest {
    name: string;      // The user's name
    email: string;     // The user's email
    password: string;  // The user's password
}

export interface UserSigninRequest {
    email: string;     // The user's email
    password: string;  // The user's password
}

export interface UserRegisterResponse {
    id: number;        // The user's ID
    role: Role;        // The user's role
    token: string;     // The generated authentication token
}

export interface UserSigninResponse {
    id: number;        // The user's ID
    role: Role;        // The user's role
    token: string;     // The generated authentication token
}

export enum Role {
    Unauthorized = 0,  // Unauthorized role
    Admin = 1,         // Admin role
    User = 2           // User role
}
