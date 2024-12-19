import { MeasurePointResponse } from "@/services/measurepoint/dtos/measurepoint_response"

export interface ComponentResponse {
    id: number
    gpioPin: number
    name: string
    color: string
    type: ComponentType
    stationId: number
    measurePoints: MeasurePointResponse[] | undefined;
}

export enum ComponentType {
    Unknown,
    Sensor,
    Actuator
}