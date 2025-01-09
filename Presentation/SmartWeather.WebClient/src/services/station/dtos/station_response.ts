import { ComponentResponse } from "@/services/component/dtos/component_response"

export interface StationDto {
    id: number
    name: string
    latitude: number
    longitude: number
    type: StationType
    userId: number
    components: ComponentResponse[] | undefined
}

export enum StationType {
    Professionnal,
    Private
}


export interface StationLatencyDetailsDto {
    station: StationDto;
    latency: number | undefined;
  }