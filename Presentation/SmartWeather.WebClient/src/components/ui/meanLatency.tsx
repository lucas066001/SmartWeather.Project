import React, { useEffect, useState } from "react";
import CardLabel from "./cardLabel";
import Arrow from "@/components/icons/Arrow";
import { twMerge } from "tailwind-merge";

interface IMeanLatencyProps {
  meanLatency?: number;
  previousMeanLatency?: number;
}

function MeanLatency({ meanLatency, previousMeanLatency }: IMeanLatencyProps) {
  const [minLatency, setMinLatency] = useState<number | undefined>(undefined);
  const [maxLatency, setMaxLatency] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (
      maxLatency === undefined ||
      (meanLatency !== undefined && meanLatency > maxLatency)
    )
      setMaxLatency(meanLatency);
    if (
      minLatency === undefined ||
      (meanLatency !== undefined && meanLatency < minLatency)
    )
      setMinLatency(meanLatency);
  }, [meanLatency]);

  return (
    <CardLabel label="Mean Latency" className="h-36">
      <div className="flex gap-4 items-center ">
        <Arrow
          up={
            meanLatency !== undefined &&
            previousMeanLatency !== undefined &&
            meanLatency > previousMeanLatency
          }
        />
        <div className="flex flex-col justify-between w-40">
          {minLatency && (
            <p className="text-disabled text-sm font-semibold">
              Min {minLatency}ms
            </p>
          )}
          <p
            className={twMerge(
              "text-3xl font-bold ",
              meanLatency !== undefined &&
                previousMeanLatency !== undefined &&
                meanLatency > previousMeanLatency
                ? "text-alert"
                : "text-primary"
            )}
          >
            {meanLatency || 0} ms
          </p>
          {maxLatency && (
            <p className="text-disabled text-sm font-semibold">
              Max {maxLatency}ms
            </p>
          )}
        </div>
      </div>
    </CardLabel>
  );
}

export default MeanLatency;
