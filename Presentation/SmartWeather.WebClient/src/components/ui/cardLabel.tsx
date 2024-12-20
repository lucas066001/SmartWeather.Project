import React, { PropsWithChildren } from "react";
import { Card, CardContent, CardHeader } from "./card";
import { twMerge } from "tailwind-merge";

interface ICardLabelProps extends PropsWithChildren{
    label: string
    className?: string
}

function CardLabel({label, children, className}: ICardLabelProps) {
  return (
    <Card className={
        twMerge("h-fit flex-1 py-2 px-4 pb-4 flex flex-col gap-2", className)
    }>
      <CardHeader className="p-0">
        <p className="font-bold text-xl text-titleBrown">{label}</p>
      </CardHeader>
      <CardContent className="p-0 ml-4 w-full h-full">
        {children}
      </CardContent>
    </Card>
  );
}

export default CardLabel;
