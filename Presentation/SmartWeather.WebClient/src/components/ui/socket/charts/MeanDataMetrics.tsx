"use client";

import React, { useEffect, useState } from "react";
import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";
import {
  StationDto,
  StationType,
} from "@/services/station/dtos/station_response";
import { Card, CardContent, CardHeader } from "../../card";
import Arrow from "@/components/icons/Arrow";
import { twMerge } from "tailwind-merge";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "../../table";
import { Input } from "../../input";
import { Button } from "../../button";
import { Map, Marker, Overlay } from "pigeon-maps";
import { CircleX, MapPin } from "lucide-react";

function MeanDataMetrics({
  lastUpdatedMeasurePoints,
  stations,
  stationMeasurePointsMap,
}: ISocketHandler) {
  const [minLatency, setMinLatency] = useState<number>(0); // :number representing the min ping between 2 station emission
  const [previousMeanLatency, setPreviousMeanLatency] = useState<number>(0);
  const [maxLatency, setMaxLatency] = useState<number>(0); // :number representing the max ping between 2 station emission
  const [meanLatency, setMeanLatency] = useState<number>(0); // :number representing the mean ping between 2 station emission
  const [meanVolume, setMeanVolume] = useState<number>(0); // :number representing the mean volume of message received in msg/sec
  const [previousMeanVolume, setPreviousMeanVolume] = useState<number>(0); // :number representing the mean volume of message received in msg/sec
  const [newDataArrived, setNewDataArrived] = useState<number>(0); // :number representing the number of message received since last volume check
  const [currentReceived, setCurrentReceived] = useState<number[]>([]); // :[ key => stationId; value => timestamp] representing the n-1 time a station has emitted
  const [previousReceived, setPreviousReceived] = useState<number[]>([]); // :[ key => stationId; value => timestamp] representing the last time a station has emitted
  const [lastVolumeCheck, setLastVolumeCheck] = useState<number>(0); // :timestamp representing the last time that volume has been checked
  const [stationsLatency, setStationsLatency] = useState<
    StationLatencyDetails[]
  >([]); // :objects representing each stations and their corresponding latency

  const [searchStation, setSearchStation] = useState("");
  const [selectedStation, setSelectedStation] = useState<
    StationDto | undefined
  >();
  interface StationLatencyDetails {
    station: StationDto;
    latency: number;
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
        const currentLatency =
          currentReceived[stationId] - previousReceived[stationId];
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

        const stationLatencyIndex: number = stationsLatency?.findIndex(
          (sm) => sm.station.id == stationId
        );

        if (stationLatencyIndex > -1) {
          stationsLatency[stationLatencyIndex].latency = currentLatency;
        } else {
          const station = stations.find((s) => s.id == stationId);
          console.log("station");
          console.log(station);

          if (station) {
            stationsLatency.push({ latency: currentLatency, station: station });
          }
        }
        const threshold = 1500;
        if (currentLatency > threshold) {
          console.log(
            "Station n°" + stationId + " has more than " + threshold + "ms"
          );
        }
      }
    });
    console.log("updateMeanLatency");
    setPreviousMeanLatency(meanLatency);
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
    setPreviousMeanVolume(previousMeanVolume);
    setMeanVolume(Number((refreshFrequency / timeElapsed).toFixed(2)));
    setLastVolumeCheck(new Date().getTime());
  };

  useEffect(() => {
    // console.log(lastUpdatedMeasurePoints.id)
    // console.log(stationMeasurePointsMap)
    const concernedStation: StationMeasurePointsResponse | undefined =
      stationMeasurePointsMap?.find((sm) =>
        sm.measurePointsIds?.includes(lastUpdatedMeasurePoints.id)
      );

    if (concernedStation) {
      const newValueData = newDataArrived + 1;
      setNewDataArrived(newValueData);
      const lastLatency: number | undefined =
        currentReceived[concernedStation.stationId];
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

    return () => {};
  }, [
    lastUpdatedMeasurePoints,
    stationMeasurePointsMap,
    lastVolumeCheck,
    currentReceived,
    previousReceived,
  ]);

  return (
    <div className="flex gap-4 items-start">
      <Card className="w-80 h-28 p-4 flex gap-4 items-center">
        <Arrow up={meanLatency > previousMeanLatency} />
        <div className="flex flex-col justify-between w-40">
          <p className="text-disabled text-sm font-semibold">
            Min {minLatency}ms
          </p>
          <p
            className={twMerge(
              "text-3xl font-bold ",
              meanLatency > previousMeanLatency ? "text-alert" : "text-primary"
            )}
          >
            {meanLatency} ms
          </p>
          <p className="text-disabled text-sm font-semibold">
            Max {maxLatency}ms
          </p>
        </div>
      </Card>

      <Card className="w-80 h-28 p-4 flex items-center justify-center">
        <CardContent className="p-0 flex gap-4 items-center">
          <Arrow up={meanVolume > previousMeanVolume} upColorGreen />
          <p
            className={twMerge(
              "text-3xl font-bold ",
              meanVolume > previousMeanVolume ? "text-primary" : "text-alert"
            )}
          >
            {meanVolume} msg / sec
          </p>
        </CardContent>
      </Card>

      <Card className="flex-1 p-4 flex gap-4 relative h-[calc(100vh-80px-1.75rem*2)]">
        <div className="flex flex-col">
          <label htmlFor="search">Filter by name</label>
          <div className="flex gap-2 mt-2">
            <Input
              id="search"
              type="search"
              className="mb-4"
              placeholder="Research ..."
              value={searchStation}
              onChange={(e) => {
                setSearchStation(e.target.value);
              }}
            />
          </div>
          <Table className="min-w-64">
            <TableHeader>
              <TableRow>
                <TableHead>Station name</TableHead>
                <TableHead>Latency</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {stationsLatency
                .filter((sl) => sl.station.name.includes(searchStation))
                .map((stationLatency) => (
                  <TableRow
                    key={stationLatency.station.id}
                    onClick={() => {
                      setSelectedStation(stationLatency.station);
                    }}
                  >
                    <TableCell>{stationLatency.station.name}</TableCell>
                    <TableCell>{stationLatency.latency}</TableCell>
                    <TableCell>
                      {stationLatency.latency <= meanLatency
                        ? "🟢"
                        : stationLatency.latency >= meanLatency * 1.15
                        ? "🔴"
                        : "🟡"}
                    </TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </div>
        <div className="w-full h-full rounded-xl overflow-hidden">
          <Map
            defaultCenter={[46.2276, 2.2137]}
            defaultZoom={5.8}
            boxClassname="rounded overflow-hidden"
          >
            {selectedStation && (
              <Overlay
                anchor={[selectedStation.latitude, selectedStation.longitude]}
              >
                <Card className="p-4 relative">
                  <CircleX
                    onClick={() => setSelectedStation(undefined)}
                    className="text-titleBrown absolute right-[-10px] top-[-10px] cursor-pointer bg-white p-1 rounded-full w-7 h-7"
                  />
                  <strong>{selectedStation.name}</strong>
                  <br />
                  Type:{" "}
                  {selectedStation.type === StationType.Professionnal
                    ? "Professional"
                    : "Private"}
                  <br />
                  ID: {selectedStation.id}
                </Card>
              </Overlay>
            )}
            {stations.map((station) => {
              return (
                <Marker
                  width={50}
                  anchor={[station.latitude, station.longitude]}
                  onClick={() => setSelectedStation(station)}
                />
              );
            })}
          </Map>
        </div>
      </Card>
    </div>
  );
}

export default MeanDataMetrics;
