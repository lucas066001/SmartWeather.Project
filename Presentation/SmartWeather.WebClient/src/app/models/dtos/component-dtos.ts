import { ComponentType } from "@constants/component-type";
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
