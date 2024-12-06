import { post } from "../api_service"
import { UserSigninRequest, UserSigninResponse } from "./dtos/signin"

const endpoints = {
    signin: "Authentication/Signin",
    register: "Authentication/Register"
}

export async function signin(email: string, password: string) {
    const content: UserSigninRequest = {
        email,
        password
    }
    return await post<UserSigninResponse>(endpoints.signin, content);
}