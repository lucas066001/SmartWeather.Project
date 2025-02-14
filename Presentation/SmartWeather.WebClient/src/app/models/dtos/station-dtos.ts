import { StationType } from "@constants/station-type";
import { ComponentResponse } from "./component-dtos";

export interface StationResponse {
    id: number;
    name: string;
    latitude: number;
    longitude: number;
    type: StationType;
    userId: number;
    components: ComponentResponse[] | null;
}

export interface StationListResponse {
    stationList: StationResponse[];
}

export interface StationClaimRequest {
    macAddress: string;
}
