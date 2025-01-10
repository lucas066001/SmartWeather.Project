import React from "react";
import {
  AreaChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  Line,
  Area,
} from "recharts";
import CardLabel from "./cardLabel";
import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "./chart";
import { MeanDataDto } from "@/services/station/dtos/mean_data";

interface IMeanDatasChartProps {
  meanDatas: MeanDataDto[];
}

const chartConfig = {
  desktop: {
    label: "volume",
    color: "#67C44D",
  },
  mobile: {
    label: "latency",
    color: "#5384AF",
  },
} satisfies ChartConfig;

function MeanDatasChart({ meanDatas }: IMeanDatasChartProps) {
  return (
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
  );
}

export default MeanDatasChart;
