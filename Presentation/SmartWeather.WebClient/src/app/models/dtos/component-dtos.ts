import { ComponentType } from "@constants/component-type";
import { MeasurePointResponse } from "@models/dtos/measurepoint-dtos";

export interface ComponentResponse {
    Id: number;
    GpioPin: number;
    Name: string;
    Color: string;
    Type: ComponentType;
    StationId: number;
    MeasurePoints: MeasurePointResponse[] | null;
}