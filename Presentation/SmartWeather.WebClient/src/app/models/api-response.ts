import { Status } from "@constants/api-status";

export class ApiResponse<T> {
    status: Status;
    message: string;
    data?: T;

    constructor(status: Status, message: string, data?: T) {
        this.status = status;
        this.message = message;
        this.data = data;
    }
}