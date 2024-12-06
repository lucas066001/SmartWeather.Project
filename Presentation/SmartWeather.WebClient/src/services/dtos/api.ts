export enum Status {
    OK,
    INTERNAL_ERROR,
    TIMEOUT_ERROR,
    DATABASE_ERROR
}


export interface ApiResponse<T extends object> {
    status: Status,
    message: string,
    data: T | null
}

