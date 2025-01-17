export enum Role {
    Admin,
    User
}

export interface UserSigninRequest {
    email: string,
    password: string
}

export interface UserSigninResponse {
    id: number,
    role: Role,
    token: string
}

export interface UserRegisterRequest {
    name : string,
    email: string,
    password: string
}

export interface UserRegisterResponse {
    id: number,
    role: Role,
    token: string
}