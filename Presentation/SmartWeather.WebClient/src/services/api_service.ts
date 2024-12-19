import { getToken } from "@/lib/tokenManager";
import { ApiResponse } from "./dtos/api";

const apiUrl = "http://localhost:8081/api/";

export async function post<T extends object>(endpoint: string, content: object | undefined): Promise<ApiResponse<T>> {
    const token = await getToken();

    const requestSpec: any = {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json",
            ...(token ? { Authorization: `Bearer ${token}` } : {})
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify(content),
    };

    const res = await fetch(apiUrl + endpoint, requestSpec);
    return await res.json()
}

export async function get<T extends object>(endpoint: string, params: object | undefined = undefined): Promise<ApiResponse<T>> {
    const token = await getToken();

    const queryString = params
        ? "?" +
        new URLSearchParams(
            Object.entries(params).reduce((acc, [key, value]) => {
                acc[key] = value.toString();
                return acc;
            }, {} as Record<string, string>)
        )
        : "";

    const requestSpec: any = {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json",
            ...(token?.value ? { Authorization: `Bearer ${token?.value}` } : {})
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
    };

    const res = await fetch(apiUrl + endpoint + queryString, requestSpec);
    return await res.json();
}