"use client";

import React from "react";
import { Input } from "./input";

interface IStationSearchBarProps {
    search: string,
    onSearchChange: (newSearch: IStationSearchBarProps["search"]) => void
}

function StationSearchBar({search, onSearchChange}: IStationSearchBarProps) {

  return (
    <div>
      <label htmlFor="search">Filter by name</label>
      <div className="flex gap-2 mt-2">
        <Input
          id="search"
          type="search"
          className="mb-4"
          placeholder="Research ..."
          value={search}
          onChange={(e) => {
            onSearchChange(e.target.value);
          }}
        />
      </div>
    </div>
  );
}

export default StationSearchBar;
