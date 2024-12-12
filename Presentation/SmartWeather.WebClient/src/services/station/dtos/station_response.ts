export interface StationDto {
    id: number
    name: string
    latitude: number
    longitude: number
    type: StationType
    userId: number
}

export enum StationType {
    Professionnal,
    Private
}
