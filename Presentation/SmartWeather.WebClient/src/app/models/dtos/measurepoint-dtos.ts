import { MeasureUnit } from "@constants/entities/measure-unit";

export interface MeasurePointResponse {
    id: number;
    name: string;
    color: string;
    unit: MeasureUnit;
    componentId: number;
}

export interface MeasurePointUpdateRequest {
    id: number;
    name: string;
    color: string;
    unit: number;
    componentId: number;
}