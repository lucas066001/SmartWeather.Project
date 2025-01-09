import React from "react";
import { Overlay } from "pigeon-maps";
import { Card } from "./card";
import { CircleX } from "lucide-react";
import { StationDto, StationType } from "@/services/station/dtos/station_response";

interface IStationOverlayProps {
  station: StationDto;
  onClose: () => void;
}

function StationInformationOverlay({ onClose, station }: IStationOverlayProps) {
  return (
    <Overlay className="z-50" anchor={[station.latitude, station.longitude]}>
      <Card className="p-4 relative ">
        <CircleX
          onClick={onClose}
          className="text-titleBrown absolute right-[-10px] top-[-10px] cursor-pointer bg-white p-1 rounded-full w-7 h-7"
        />
        <strong>{station.name}</strong>
        <br />
        Type:{" "}
        {station.type === StationType.Professionnal
          ? "Professional"
          : "Private"}
        <br />
        ID: {station.id}
      </Card>
    </Overlay>
  );
}

export default StationInformationOverlay;
