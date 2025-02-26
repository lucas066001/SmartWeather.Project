
export interface MeasureDataResponse {
    measurePointId: number;
    value: number;
    dateTime: Date;
}


export interface MeasureDataListResponse {
    measureDataList: MeasureDataResponse[];
}
