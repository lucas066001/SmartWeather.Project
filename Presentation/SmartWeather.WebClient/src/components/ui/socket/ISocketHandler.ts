import { MeasurePointDataDto } from "@/services/dtos/socket";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";
import { StationDto } from "@/services/station/dtos/station_response";

export interface ISocketHandler {
    lastUpdatedMeasurePoints: MeasurePointDataDto;
    stations: StationDto[];
    stationMeasurePointsMap: StationMeasurePointsResponse[];
}