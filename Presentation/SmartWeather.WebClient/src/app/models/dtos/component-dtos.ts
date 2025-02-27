import { ComponentType } from "@constants/entities/component-type";
import { MeasurePointResponse } from "@models/dtos/measurepoint-dtos";

export interface ComponentResponse {
    id: number;
    gpioPin: number;
    name: string;
    color: string;
    type: ComponentType;
    stationId: number;
    measurePoints: MeasurePointResponse[] | null;
}

export interface ComponentListResponse {
    componentList: ComponentResponse[] | null;
}

export interface ComponentUpdateRequest {
    id: number;
    gpioPin: number;
    name: string;
    color: string;
    type: number;
    stationId: number;
}

export interface ActuatorCommandRequest {
    stationId: number;
    componentGpioPin: number;
    componentValueUpdate: number;
}