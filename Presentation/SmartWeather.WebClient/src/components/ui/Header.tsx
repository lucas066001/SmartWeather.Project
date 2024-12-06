"use client"
import { logout } from "@/actions/auth";
import React from "react";

export interface IHeaderProps {
  title?: string;
}

function Header({ title }: IHeaderProps) {
  return (
    <header className="w-full border-b h-20 bg-white flex items-center justify-center">
      {title && (
        <h1 className="font-semibold text-titleBrown text-3xl">{title}</h1>
      )}
        <button onClick={() => logout()}>Submit</button>
    </header>
  );
}

export default Header;
