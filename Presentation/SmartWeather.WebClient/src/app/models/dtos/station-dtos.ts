import { StationType } from "@constants/entities/station-type";
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

export interface StationUpdateRequest {
    id: number;
    name: string;
    latitude: number;
    longitude: number;
    userId: number;
}