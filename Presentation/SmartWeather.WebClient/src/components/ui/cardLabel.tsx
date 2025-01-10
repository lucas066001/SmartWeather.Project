import { PropsWithChildren } from "react";
import { twMerge } from "tailwind-merge";
import { Card, CardContent, CardHeader } from "./card";

interface ICardLabelProps extends PropsWithChildren{
    label: string
    className?: string
    contentClassName?: string
}

function CardLabel({label, children, className, contentClassName}: ICardLabelProps) {
  return (
    <Card className={
        twMerge("h-fit flex-1 py-2 px-4 pb-4 flex flex-col gap-2", className)
    }>
      <CardHeader className="p-0">
        <p className="font-bold text-xl text-titleBrown">{label}</p>
      </CardHeader>
      <CardContent className={twMerge("p-0 ml-4 w-full h-full",contentClassName)}>
        {children}
      </CardContent>
    </Card>
  );
}

export default CardLabel;
