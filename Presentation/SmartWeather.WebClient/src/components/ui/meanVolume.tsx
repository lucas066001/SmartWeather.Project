import React from "react";
import CardLabel from "./cardLabel";
import Arrow from "../icons/Arrow";
import { twMerge } from "tailwind-merge";

interface IMeanVolumeProps {
  meanVolume?: number;
  previousMeanVolume?: number;
}

function MeanVolume({ meanVolume, previousMeanVolume }: IMeanVolumeProps) {
  return (
    <CardLabel label="Mean Volume" className="h-36">
      <div className="flex gap-4 items-center ">
        <Arrow
          up={
            meanVolume !== undefined &&
            previousMeanVolume != undefined &&
            meanVolume > previousMeanVolume
          }
          upColorGreen
        />
        <p
          className={twMerge(
            "text-3xl font-bold ",
            meanVolume !== undefined &&
              previousMeanVolume != undefined &&
              meanVolume > previousMeanVolume
              ? "text-primary"
              : "text-alert"
          )}
        >
          {meanVolume || 0} msg / sec
        </p>
      </div>
    </CardLabel>
  );
}

export default MeanVolume;
