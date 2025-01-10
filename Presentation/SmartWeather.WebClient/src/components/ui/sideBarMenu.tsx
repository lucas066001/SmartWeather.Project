"use client";

import React, { useState } from "react";
import MenuLink, { ILink } from "./menuLink";
import Image from "next/image";
import extendedLogo from "../../../public/images/logo-extended.svg";
import compactedLogo from "../../../public/images/logo-compacted.svg";
import arrowLeft from "../../../public/icons/arrow-left.svg";
import { twMerge } from "tailwind-merge";
import { usePathname } from "next/navigation";

interface ISidebarMenuProps {
  menu: ILink[];
}

function SideBarMenu({ menu }: ISidebarMenuProps) {
  const [extended, setExtended] = useState(true);
  const pathname = usePathname();

  return (
    <div
      className={twMerge(
        "relative flex flex-col justify-between items-center h-screen border-r pt-10 font-outfit font-medium transition-all duration-300 w-64 bg-white",
        !extended && "w-28"
      )}
    >
      <div
        className="absolute bg-white border rounded-xl w-9 h-9 flex items-center justify-center right-[-18px] top-[128px] cursor-pointer hover:bg-slate-100 transition-all"
        onClick={() => {
          setExtended((curr) => !curr);
        }}
      >
        <Image
          src={arrowLeft}
          alt=""
          className={`transition ${!extended ? "rotate-180" : ""}`}
        />
      </div>

      <Image
        src={extended ? extendedLogo : compactedLogo}
        alt="Logo"
        className={twMerge(!extended && "w-16")}
      />

      <nav className="w-full flex flex-col gap-3 items-center">
        {menu.map((linkProps, id) => (
          <MenuLink
            active={linkProps.href === pathname}
            key={id}
            extended={extended}
            {...linkProps}
          />
        ))}
      </nav>
      <div className="px-6 py-4 border-t w-full">
        <p className="text-xs text-center text-[#e5e7eb]">
          © 2024 SmartWeather, all right reserved
        </p>
      </div>
    </div>
  );
}

export default SideBarMenu;
