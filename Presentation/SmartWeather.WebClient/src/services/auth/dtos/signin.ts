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