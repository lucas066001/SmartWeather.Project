import React from "react";
import { twMerge } from "tailwind-merge";

interface IArrowProps {
  up: boolean;
  upColorGreen?: boolean;
}

function Arrow({ up, upColorGreen }: IArrowProps) {
  return (
    <svg
      width="42"
      height="26"
      viewBox="0 0 21 13"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      className={twMerge(
        "transition-all",
        !upColorGreen
          ? up
            ? "fill-alert "
            : "fill-primary rotate-180"
          : up
          ? "fill-primary "
          : "fill-alert rotate-180"
      )}
    >
      <path d="M9.73892 0.892998C10.1382 0.424538 10.8618 0.424537 11.2611 0.892997L19.9202 11.053C20.4735 11.7022 20.0122 12.7016 19.1591 12.7016H1.84088C0.987847 12.7016 0.526473 11.7022 1.0798 11.053L9.73892 0.892998Z" />
    </svg>
  );
}

export default Arrow;
