import React from "react";
import { Map, Marker, ZoomControl } from "pigeon-maps";
import StationInformationOverlay from "./stationInformationOverlay";
import { StationDto } from "@/services/station/dtos/station_response";

interface IStationsMapProps {
  stations: StationDto[];
  selectedStation?: StationDto;
  onSelectedStationChange: (
    newSelectedStation: IStationsMapProps["selectedStation"]
  ) => void;
}

function StationsMap({
  stations,
  selectedStation,
  onSelectedStationChange,
}: IStationsMapProps) {
  return (
    <>
      {window && (
        <Map
          defaultCenter={[46.2276, 2.2137]}
          center={
            selectedStation && [
              selectedStation.latitude,
              selectedStation.longitude,
            ]
          }
          defaultZoom={5.8}
          zoom={selectedStation && 10}
          zoomSnap
          boxClassname="rounded overflow-hidden"
        >
          {selectedStation && (
            <StationInformationOverlay
              station={selectedStation}
              onClose={() => onSelectedStationChange(undefined)}
            />
          )}

          {stations.map((station) => {
            return (
              <Marker
                key={station.id}
                className="z-10"
                width={50}
                anchor={[station.latitude, station.longitude]}
                onClick={() => onSelectedStationChange(station)}
              />
            );
          })}
          <ZoomControl />
        </Map>
      )}
    </>
  );
}

export default StationsMap;
