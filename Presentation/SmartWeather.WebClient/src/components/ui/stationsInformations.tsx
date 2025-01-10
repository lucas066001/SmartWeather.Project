"use client";

import React, { useMemo, useState } from "react";
import CardLabel from "./cardLabel";
import StationsMap from "./stationsMap";
import StationSearchBar from "./stationSearchBar";
import {
  StationDto,
  StationLatencyDetailsDto,
} from "@/services/station/dtos/station_response";
import StationsTable, { IStationRow } from "./stationsTable";

interface IStationInformationsProps {
  stationsLatency: StationLatencyDetailsDto[];
  stations: StationDto[];
  meanLatency: number;
}

function StationsInformations({
  stationsLatency,
  stations,
  meanLatency,
}: IStationInformationsProps) {
  const [search, setSearch] = useState("");
  const [selectedStation, setSelectedStation] = useState<
    StationDto | undefined
  >(undefined);

  function handleSlectedRow(id: number) {
    setSelectedStation(stations.find((station) => station.id == id));
  }

  const rows: IStationRow[] = useMemo(
    () =>
      stationsLatency
        .filter((sl) => sl.station.name.includes(search))
        .map(({ station: { name, id }, latency }) => ({
          name,
          id,
          latency,
          selected: id === selectedStation?.id,
          state: latency
            ? latency <= meanLatency
              ? "🟢"
              : latency >= meanLatency * 1.15
              ? "🔴"
              : "🟡"
            : "⚫",
        })),
    [stationsLatency, search, selectedStation]
  );

  return (
    <CardLabel
      label="Stations list"
      className="h-[calc(100vh-80px-1.75rem*2)] w-1/2 flex-none"
    >
      <div className="p-4 flex gap-4 h-full w-full">
        <div className="flex flex-col">
          <StationSearchBar search={search} onSearchChange={setSearch} />
          <StationsTable rows={rows} onSelectedRow={handleSlectedRow} />
        </div>

        <div className="h-full rounded-xl overflow-hidden flex-1 ">
          <StationsMap
            stations={stations}
            onSelectedStationChange={setSelectedStation}
            selectedStation={selectedStation}
          />
        </div>
      </div>
    </CardLabel>
  );
}

export default StationsInformations;
