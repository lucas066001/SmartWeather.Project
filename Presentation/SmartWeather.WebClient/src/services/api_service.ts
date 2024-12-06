import { ApiResponse } from "./dtos/api";

const apiUrl = "http://localhost:8081/api/";

export async function post<T extends object>(endpoint: string, content: object): Promise<ApiResponse<T>> {
    const requestSpec: any = {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json",
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify(content),
    };

    const res = await fetch(apiUrl + endpoint, requestSpec);
    return await res.json()
}