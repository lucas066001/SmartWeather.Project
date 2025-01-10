"use client";

import React, { useEffect, useState } from "react";
import { ISocketHandler } from "@/components/ui/socket/ISocketHandler";
import { StationMeasurePointsResponse } from "@/services/station/dtos/station_measure_points_response";
import { StationLatencyDetailsDto } from "@/services/station/dtos/station_response";
import Arrow from "@/components/icons/Arrow";
import { twMerge } from "tailwind-merge";

import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "../../chart";
import {
  CartesianGrid,
  XAxis,
  Area,
  AreaChart,
  YAxis,
  Tooltip,
  Legend,
  Line,
} from "recharts";
import CardLabel from "../../cardLabel";
import StationsInformations from "../../stationsInformations";

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
  const [meanDatas, setMeanDatas] = useState<MeanData[]>([]); // :timestamp representing the last time that volume has been checked
  const [stationsLatency, setStationsLatency] = useState<
    StationLatencyDetailsDto[]
  >([]); // :objects representing each stations and their corresponding latency

  interface MeanData {
    time: Date;
    latency: number;
    volume: number;
  }

  const refreshFrequency: number = 3;

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
          const newStationsLatency = [...stationsLatency];
          newStationsLatency[stationLatencyIndex].latency = currentLatency;
          setStationsLatency(newStationsLatency);
        } else {
          const station = stations.find((s) => s.id == stationId);

          if (station) {
            setStationsLatency((curr) => [
              ...curr,
              { station: station, latency: currentLatency },
            ]);
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
    setPreviousMeanVolume(meanVolume);
    const currentVolume = Number((refreshFrequency / timeElapsed).toFixed(2));
    setMeanDatas([
      ...meanDatas,
      { time: new Date(), volume: currentVolume, latency: meanLatency },
    ]);
    setMeanVolume(currentVolume);
    setLastVolumeCheck(new Date().getTime());
  };

  useEffect(() => {
    stations.forEach((station) => {
      setStationsLatency((curr) => [
        ...curr,
        { station: station, latency: undefined },
      ]);
    });
    return () => {};
  }, [stations]);

  useEffect(() => {
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

  const chartConfig = {
    desktop: {
      label: "Desktop",
      color: "#67C44D",
    },
    mobile: {
      label: "Mobile",
      color: "#5384AF",
    },
  } satisfies ChartConfig;

  return (
    <div className="flex gap-4 items-start">
      <div className="flex flex-col gap-4 h-[calc(100vh-80px-1.75rem*2)]">
        <div className="flex gap-4 items-center w-full">
          <CardLabel label="Mean Latency" className="h-36">
            <div className="flex gap-4 items-center ">
              <Arrow up={meanLatency > previousMeanLatency} />
              <div className="flex flex-col justify-between w-40">
                <p className="text-disabled text-sm font-semibold">
                  Min {minLatency}ms
                </p>
                <p
                  className={twMerge(
                    "text-3xl font-bold ",
                    meanLatency > previousMeanLatency
                      ? "text-alert"
                      : "text-primary"
                  )}
                >
                  {meanLatency} ms
                </p>
                <p className="text-disabled text-sm font-semibold">
                  Max {maxLatency}ms
                </p>
              </div>
            </div>
          </CardLabel>

          <CardLabel label="Mean Volume" className="h-36">
            <div className="flex gap-4 items-center ">
              <Arrow up={meanVolume > previousMeanVolume} upColorGreen />
              <p
                className={twMerge(
                  "text-3xl font-bold ",
                  meanVolume > previousMeanVolume
                    ? "text-primary"
                    : "text-alert"
                )}
              >
                {meanVolume} msg / sec
              </p>
            </div>
          </CardLabel>
        </div>

        <CardLabel label="Chart" className="w-full h-full p-4">
          <ChartContainer config={chartConfig}>
            <AreaChart
              accessibilityLayer
              data={meanDatas}
              margin={{
                left: 12,
                right: 12,
              }}
            >
              <CartesianGrid vertical={false} />
              <XAxis
                dataKey="time"
                tickLine={false}
                axisLine={false}
                tickMargin={8}
                tickFormatter={(value: Date) => value.toLocaleTimeString()}
              />
              <YAxis yAxisId="left" />
              <YAxis yAxisId="right" orientation="right" />
              <Tooltip />
              <Legend />
              <Line
                yAxisId="left"
                type="monotone"
                dataKey="volume"
                stroke="#8884d8"
                activeDot={{ r: 8 }}
              />
              <Line
                yAxisId="right"
                type="monotone"
                dataKey="latency"
                stroke="#82ca9d"
              />
              <ChartTooltip
                cursor={false}
                content={<ChartTooltipContent indicator="dot" />}
              />
              <Area
                dataKey="volume"
                type="natural"
                fill="var(--color-mobile)"
                fillOpacity={0.4}
                stroke="var(--color-mobile)"
                stackId="a"
                yAxisId={"left"}
              />
              <Area
                dataKey="latency"
                type="natural"
                fill="var(--color-desktop)"
                fillOpacity={0.4}
                stroke="var(--color-desktop)"
                stackId="a"
                yAxisId={"right"}
              />
            </AreaChart>
          </ChartContainer>
        </CardLabel>
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
