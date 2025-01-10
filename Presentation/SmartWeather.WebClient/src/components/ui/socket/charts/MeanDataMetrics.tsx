"use client";

import React, { useEffect, useMemo, useState } from "react";
import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";
import { StationLatencyDetailsDto } from "@/services/station/dtos/station_response";
import StationsInformations from "../../stationsInformations";
import MeanLatency from "../../meanLatency";
import MeanVolume from "../../meanVolume";
import MeanDatasChart from "../../meanDatasChart";
import { MeanDataDto } from "@/services/station/dtos/mean_data";

const refreshFrequency: number = 30;

function MeanDataMetrics({
  lastUpdatedMeasurePoints,
  stations,
  stationMeasurePointsMap,
}: ISocketHandler) {
  const [lastVolumeCheck, setLastVolumeCheck] = useState<number>(0); // :timestamp representing the last time that volume has been checked
  const [meanDatas, setMeanDatas] = useState<MeanDataDto[]>([]); // :timestamp representing the last time that volume has been checked

  // meanLatency: Number representing the mean ping between 2 station emission
  const meanLatency = useMemo(
    () =>
      meanDatas.length > 1
        ? meanDatas[meanDatas.length - 1].latency
        : undefined,
    [meanDatas]
  );
  const previousMeanLatency = useMemo(
    () =>
      meanDatas.length > 2
        ? meanDatas[meanDatas.length - 2].latency
        : undefined,
    [meanDatas]
  );

  // meanVolume: Number representing the mean volume of message received in msg/sec
  const meanVolume = useMemo(
    () =>
      meanDatas.length > 1 ? meanDatas[meanDatas.length - 1].volume : undefined,
    [meanDatas]
  );
  const previousMeanVolume = useMemo(
    () =>
      meanDatas.length > 2 ? meanDatas[meanDatas.length - 2].volume : undefined,
    [meanDatas]
  );

  const [stationsLatency, setStationsLatency] = useState<
    StationLatencyDetailsDto[]
  >([]); // :objects representing each stations and their corresponding latency

  const [messageNbSinceLastRefresh, setMessageNbSinceLastRefresh] =
    useState<number>(0); // :number representing the number of message received since last volume check
  const [currentReceived, setCurrentReceived] = useState<{
    [stationId: number]: number;
  }>({}); // :[ key => stationId; value => timestamp] representing the n-1 time a station has emitted
  const [previousReceived, setPreviousReceived] = useState<{
    [stationId: number]: number;
  }>({}); // :[ key => stationId; value => timestamp] representing the last time a station has emitted

  useEffect(() => {
    const concernedStation: StationMeasurePointsResponse | undefined =
      stationMeasurePointsMap?.find((sm) =>
        sm.measurePointsIds?.includes(lastUpdatedMeasurePoints.id)
      );
    if (concernedStation) {
      const currentDate = new Date();
      const lastLatency: number | undefined =
        currentReceived[concernedStation.stationId];

      setCurrentReceived((curr) => ({
        ...curr,
        [concernedStation.stationId]: currentDate.getTime(),
      }));
      setPreviousReceived((curr) => ({
        ...curr,
        [concernedStation.stationId]: !lastLatency
          ? currentDate.getTime()
          : lastLatency,
      }));

      let newMeanVolume: number | undefined = meanVolume;
      if (messageNbSinceLastRefresh >= refreshFrequency) {
        const timeElapsed = (new Date().getTime() - lastVolumeCheck) / 1000;
        newMeanVolume = Number((refreshFrequency / timeElapsed).toFixed(2));
        setMessageNbSinceLastRefresh(0);
        setLastVolumeCheck(currentDate.getTime());
        let nbStationChecked: number = 0;
        let sumLatencies: number = 0;
        Object.keys(currentReceived).forEach((stationId: string) => {
          if (previousReceived[Number(stationId)]) {
            const currentLatency =
              currentReceived[Number(stationId)] -
              previousReceived[Number(stationId)];
            if (currentLatency != 0) {
              nbStationChecked++;
              sumLatencies += currentLatency;
            }

            const stationLatencyIndex: number = stationsLatency?.findIndex(
              (sm) => sm.station.id == Number(stationId)
            );

            if (stationLatencyIndex > -1) {
              const newStationsLatency = [...stationsLatency];
              newStationsLatency[stationLatencyIndex].latency = currentLatency;
              setStationsLatency(newStationsLatency);
            } else {
              const station = stations.find((s) => s.id == Number(stationId));

              if (station) {
                setStationsLatency((curr) => [
                  ...curr,
                  { station: station, latency: currentLatency },
                ]);
              }
            }
          }
        });
        const latency = Number((sumLatencies / nbStationChecked).toFixed(2));
        setMeanDatas((curr) => [
          ...curr,
          {
            time: currentDate,
            latency: latency || 0,
            volume: newMeanVolume || 0,
          },
        ]);
      }
      setMessageNbSinceLastRefresh((curr) => curr + 1);
    }
  }, [lastUpdatedMeasurePoints]);

  return (
    <div className="flex gap-4 items-start">
      <div className="flex flex-col gap-4 h-[calc(100vh-80px-1.75rem*2)]">
        <div className="flex gap-4 items-center w-full">
          <MeanLatency
            meanLatency={meanLatency}
            previousMeanLatency={previousMeanLatency}
          />

          <MeanVolume
            meanVolume={meanVolume}
            previousMeanVolume={previousMeanVolume}
          />
        </div>

        <MeanDatasChart meanDatas={meanDatas} />
      </div>

      <StationsInformations
        stations={stations}
        stationsLatency={stationsLatency}
        meanLatency={meanLatency}
      />
    </div>
  );
}

export default MeanDataMetrics;
