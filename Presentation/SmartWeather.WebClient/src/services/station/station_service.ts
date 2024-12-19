import { get, post } from "../api_service"
import { getToken } from '@/lib/tokenManager';
import { StationDto } from "./dtos/station_response";
import { ApiResponse } from "../dtos/api";
import { StationListDto } from "./dtos/station_list";
import { StationMeasurePointsResponse } from "./dtos/station_measure_points_response";

const endpoints = {
    getAll: "Station/GetAll",
    getStationsMeasurePoints: "Station/GetStationsMeasurePoints",
}

export async function getAllStation(includeComponents: boolean = false, includeMeasurePoints: boolean = false): Promise<ApiResponse<StationListDto>> {
    return await get<StationListDto>(endpoints.getAll, { includeComponents: includeComponents, includeMeasurePoints: includeMeasurePoints });
}

export async function getStationsMeasurePoints(): Promise<ApiResponse<StationMeasurePointsResponse[]>> {
    return await get<StationMeasurePointsResponse[]>(endpoints.getStationsMeasurePoints);
}