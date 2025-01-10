"use client";

import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "./table";
import { twMerge } from "tailwind-merge";

export interface IStationRow {
  id: number;
  name: string;
  latency?: number;
  state: string;
  selected?: boolean;
}

interface IStationsTableProps {
  rows: IStationRow[];
  onSelectedRow: (id: number) => void;
}

function StationsTable({ rows, onSelectedRow }: IStationsTableProps) {
  return (
    <Table className="min-w-64">
      <TableHeader>
        <TableRow>
          <TableHead>Station name</TableHead>
          <TableHead>Latency</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {rows.map(({ id, selected, name, latency, state }) => (
          <TableRow
            key={id}
            onClick={() => {
              onSelectedRow(id);
            }}
            className={twMerge("cursor-pointer", selected && "font-semibold")}
          >
            <TableCell>{name}</TableCell>
            <TableCell>{latency}</TableCell>
            <TableCell>{state}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}

export default StationsTable;
