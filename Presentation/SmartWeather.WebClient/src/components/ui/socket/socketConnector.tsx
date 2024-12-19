"use client"
import React, { useEffect, useState } from 'react'
import { getToken } from '@/lib/tokenManager';
import { Status } from '@/services/dtos/api';
import { StationDto } from '@/services/station/dtos/station_response';
import { MeasurePointDataDto, MonitoringRequest } from '@/services/dtos/socket';
import { signalRService } from '@/services/socket_service';
import { getAllStation, getStationsMeasurePoints } from '@/services/station/station_service';
import { StationMeasurePointsResponse } from '@/services/station/dtos/station_measure_points_response';
import MeanDataMetrics from '@/components/ui/socket/charts/MeanDataMetrics';

// Dynamically import `StationsMapEmitting` to disable SSR
// const StationsMapEmitting = dynamic(() => import('@/components/ui/socket/charts/StationsMapEmitting'), { ssr: false });

function socketConnector() {
    const [lastUpdatedMeasurePoints, setlastUpdatedMeasurePoints] = useState<MeasurePointDataDto>({ id: 0, value: 0 });
    const [stationMeasurePointsMap, setStationMeasurePointsMap] = useState<StationMeasurePointsResponse[]>([]);
    const [stations, setStations] = useState<StationDto[]>([]);

    useEffect(() => {
        const initStations = async () => {
            let stationsResponse = await getAllStation(true, true);
            if (stationsResponse.status == Status.OK && stationsResponse.data != null) {
                setStations(stationsResponse.data.stationList);
            }
        }
        initStations()
        return () => { };
    }, []);

    useEffect(() => {
        const initStationsMapping = async () => {
            let stationsResponse = await getStationsMeasurePoints();
            if (stationsResponse.status == Status.OK && stationsResponse.data != null) {
                console.log(stationsResponse)
                setStationMeasurePointsMap(stationsResponse.data);
            }
        }
        initStationsMapping()
        return () => { };
    }, []);

    useEffect(() => {
        const initSocketConnection = async () => {
            await signalRService.startConnection();
            const cookie = await getToken();
            const token: string = cookie?.value || "";

            const monitoringReq: MonitoringRequest = {
                token: token
            }

            signalRService.invoke<MeasurePointDataDto>("InitiateMonitoring", monitoringReq)
            signalRService.on("receivedMeasurePointData", (newMeasurePoint: MeasurePointDataDto) => {
                setlastUpdatedMeasurePoints(newMeasurePoint);
            });
        };

        initSocketConnection();

        return () => {
            signalRService.stopConnection();
        };
    }, [stations]);

    return (
        <div>
            <MeanDataMetrics stationMeasurePointsMap={stationMeasurePointsMap} lastUpdatedMeasurePoints={lastUpdatedMeasurePoints} stations={stations} />
            {/* <StationsMapEmitting dataPoints={measurePoints} stations={stations} /> */}
        </div>
    );
}

export default socketConnector