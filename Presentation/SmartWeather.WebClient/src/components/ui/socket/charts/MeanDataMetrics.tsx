"use client";

import React, { useEffect, useState } from "react";
import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";
import { record } from "zod";
import { StationDto } from "@/services/station/dtos/station_response";

function MeanDataMetrics({ lastUpdatedMeasurePoints, stations, stationMeasurePointsMap }: ISocketHandler) {
    const [minLatency, setMinLatency] = useState<number>(0);                                // :number representing the min ping between 2 station emission
    const [maxLatency, setMaxLatency] = useState<number>(0);                                // :number representing the max ping between 2 station emission
    const [meanLatency, setMeanLatency] = useState<number>(0);                              // :number representing the mean ping between 2 station emission
    const [meanVolume, setMeanVolume] = useState<number>(0);                                // :number representing the mean volume of message received in msg/sec
    const [newDataArrived, setNewDataArrived] = useState<number>(0);                        // :number representing the number of message received since last volume check
    const [currentReceived, setCurrentReceived] = useState<number[]>([]);                   // :[ key => stationId; value => timestamp] representing the n-1 time a station has emitted
    const [previousReceived, setPreviousReceived] = useState<number[]>([]);                 // :[ key => stationId; value => timestamp] representing the last time a station has emitted
    const [lastVolumeCheck, setLastVolumeCheck] = useState<number>(0);                      // :timestamp representing the last time that volume has been checked
    const [stationsLatency, setStationsLatency] = useState<StationLatencyDetails[]>([]);    // :objects representing each stations and their corresponding latency

    interface StationLatencyDetails {
        station: StationDto
        latency: number
        volume: number
    }

    const refreshFrequency: number = 30;

    const updateLatency = () => {
        let nbStationChecked: number = 0;
        let sumLatencies: number = 0;
        let minLatency: number | undefined = undefined;
        let maxLatency: number | undefined = undefined;
        console.log(currentReceived);
        console.log(previousReceived);

        currentReceived.keys().forEach((stationId: number) => {
            if (previousReceived[stationId]) {
                const currentLatency = currentReceived[stationId] - previousReceived[stationId];
                if (currentLatency != 0) {
                    nbStationChecked++;
                    sumLatencies += currentLatency;
                    if (maxLatency == undefined || currentLatency > maxLatency) {
                        maxLatency = currentLatency;
                    }
                    if (minLatency == undefined || currentLatency < minLatency) {
                        minLatency = currentLatency;
                    }
                }

                const stationLatencyIndex: number = stationsLatency?.findIndex((sm) =>
                    sm.station.id == stationId);

                if (stationLatencyIndex > -1) {
                    stationsLatency[stationLatencyIndex].latency = currentLatency;
                } else {
                    const station = stations.find((s) => s.id == stationId);
                    console.log("station");
                    console.log(station);

                    if (station) {
                        stationsLatency.push({ latency: currentLatency, station: station })
                    }
                }
                const threshold = 1500;
                if (currentLatency > threshold) {
                    console.log("Station n°" + stationId + " has more than " + threshold + "ms")
                }
            }
        })
        console.log("updateMeanLatency");
        setMeanLatency(Number((sumLatencies / nbStationChecked).toFixed(2)));
        if (minLatency) {
            setMinLatency(minLatency);
        }
        if (maxLatency) {
            setMaxLatency(maxLatency);
        }
    };

    const updateMeanVolume = () => {
        const timeElapsed = (new Date().getTime() - lastVolumeCheck) / 1000;
        setMeanVolume(Number((refreshFrequency / timeElapsed).toFixed(2)));
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
            const lastLatency: number | undefined = currentReceived[concernedStation.stationId];
            if (!lastLatency) {
                previousReceived[concernedStation.stationId] = new Date().getTime();
                currentReceived[concernedStation.stationId] = new Date().getTime();
            } else {
                previousReceived[concernedStation.stationId] = lastLatency;
                currentReceived[concernedStation.stationId] = new Date().getTime();
            }
            // console.log("station n°" + concernedStation.stationId + "gets updates");
            // console.log("newDataArrived =" + newDataArrived);

            if (newDataArrived >= refreshFrequency) {
                updateLatency();
                updateMeanVolume();
                setNewDataArrived(0);
            }
        }

        return () => {
        };
    }, [lastUpdatedMeasurePoints, stationMeasurePointsMap, lastVolumeCheck, currentReceived, previousReceived]);

    return (
        <div>
            <h1>Max : {maxLatency}ms</h1>
            <h1>Min : {minLatency}ms</h1>
            <h1>Mean : {meanLatency}ms</h1>
            <table>
                <tr>
                    <td>Station Name</td>
                    <td>Latency</td>
                </tr>
                {
                    stationsLatency.map((stationLatency) => (
                        <tr>
                            <td>{stationLatency.station.name}</td>
                            <td>{stationLatency.latency}</td>
                        </tr>
                    ))
                }
            </table>

            <h1>{meanVolume}msg/sec</h1>
        </div>
    );
}

export default MeanDataMetrics;
