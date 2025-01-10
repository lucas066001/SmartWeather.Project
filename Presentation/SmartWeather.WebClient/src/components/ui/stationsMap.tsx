import React from "react";
import { Map, Marker, Overlay, ZoomControl } from "pigeon-maps";
import {
  StationDto,
  StationType,
} from "@/services/station/dtos/station_response";
import { Card } from "./card";
import { CircleX } from "lucide-react";

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
  console.log(selectedStation);
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
            <Overlay
              className="z-50"
              anchor={[selectedStation.latitude, selectedStation.longitude]}
            >
              <Card className="p-4 relative ">
                <CircleX
                  className="text-titleBrown absolute right-[-10px] top-[-10px] cursor-pointer bg-white p-1 rounded-full w-7 h-7"
                  onClick={() => {
                    onSelectedStationChange(undefined);
                  }}
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
