import { MeasureUnit } from "@constants/measure-unit";

export interface MeasurePointResponse {
    id: number;
    name: string;
    color: string;
    unit: MeasureUnit;
    componentId: number;
}
