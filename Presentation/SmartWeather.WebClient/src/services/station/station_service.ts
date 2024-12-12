import { get, post } from "../api_service"
import { getToken } from '@/lib/tokenManager';
import { StationDto } from "./dtos/station_response";
import { ApiResponse } from "../dtos/api";
import { StationListDto } from "./dtos/station_list";

const endpoints = {
    getAll: "Station/GetAll"
}

export async function getAllStation(includeComponents: boolean = false, includeMeasurePoints: boolean = false): Promise<ApiResponse<StationListDto>> {
    return await get<StationListDto>(endpoints.getAll, { includeComponents: includeComponents, includeMeasurePoints: includeMeasurePoints });
}