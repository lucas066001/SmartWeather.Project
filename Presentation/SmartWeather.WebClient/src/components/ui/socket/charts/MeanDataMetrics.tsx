"use client";

import React, { useEffect, useState } from "react";
import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";

function MeanDataMetrics({ lastUpdatedMeasurePoints, stations, stationMeasurePointsMap }: ISocketHandler) {
    const [meanLatency, setMeanLatency] = useState<number>(0);                  // :number representing the mean ping between 2 station emission
    const [meanVolume, setMeanVolume] = useState<number>(0);                    // :number representing the mean volume of message received in msg/sec
    const [newDataArrived, setNewDataArrived] = useState<number>(0);            // :number representing the number of message received since last volume check
    const [currentReceived, setCurrentReceived] = useState<number[]>([]);       // :[ key => stationId; value => timestamp] representing the n-1 time a station has emitted
    const [previousReceived, setPreviousReceived] = useState<number[]>([]);     // :[ key => stationId; value => timestamp] representing the last time a station has emitted
    const [lastVolumeCheck, setLastVolumeCheck] = useState<number>(0);          // :timestamp representing the last time that volume has been checked
    const refreshFrequency: number = 20;

    const updateMeanLatency = () => {
        let nbStationChecked: number = 0;
        let sumLatencies: number = 0;
        console.log(currentReceived);
        console.log(previousReceived);

        currentReceived.keys().forEach((stationId: number) => {
            if (previousReceived[stationId]) {
                nbStationChecked++;
                sumLatencies += currentReceived[stationId] - previousReceived[stationId];
            }
        })
        console.log("updateMeanLatency");
        setMeanLatency(Number((sumLatencies / nbStationChecked).toFixed(2)));
    };

    const updateMeanVolume = () => {
        const timeElapsed = (new Date().getTime() - lastVolumeCheck) / 10e3;
        setMeanVolume(refreshFrequency / timeElapsed);
        setLastVolumeCheck(new Date().getTime());
    };

    useEffect(() => {
        // console.log(lastUpdatedMeasurePoints.id)
        // console.log(stationMeasurePointsMap)
        const concernedStation: StationMeasurePointsResponse | undefined = stationMeasurePointsMap?.find((sm) =>
            sm.measurePointsIds?.includes(lastUpdatedMeasurePoints.id));

        if (concernedStation) {
            const newValueData = newDataArrived + 1;
            setNewDataArrived(newValueData);
            const lastLatency: number = currentReceived[concernedStation.stationId] ?? 0;
            previousReceived[concernedStation.stationId] = lastLatency;
            currentReceived[concernedStation.stationId] = new Date().getTime();
            // console.log("station n°" + concernedStation.stationId + "gets updates");
            // console.log("newDataArrived =" + newDataArrived);

            if (newDataArrived >= refreshFrequency) {
                updateMeanLatency();
                updateMeanVolume();
                setNewDataArrived(0);
            }
        }

        return () => {
        };
    }, [lastUpdatedMeasurePoints, stationMeasurePointsMap]);

    return (
        <div>
            <h1>{meanLatency}ms</h1>
            <h1>{meanVolume}msg/sec</h1>
        </div>
    );
}

export default MeanDataMetrics;
