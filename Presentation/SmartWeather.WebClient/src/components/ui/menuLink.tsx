import Link from "next/link";
import React, { ComponentType } from "react";
import { twMerge } from "tailwind-merge";
import { IIcon } from "../icons/type";

export interface ILink {
  icon: ComponentType<IIcon>;
  label: string;
  href: string;
}

interface IMenuLinkProps extends ILink {
  extended: boolean;
  active: boolean;
}

function MenuLink({
  active,
  extended,
  icon: Icon,
  label,
  href,
}: IMenuLinkProps) {
  return (
    <Link
      href={href}
      className={twMerge(
        "relative w-full h-16 flex items-center gap-8 text-lg text-disabled pl-9",
        active && "text-primary",
        !extended &&
          "pl-0 justify-center gap-0 w-14 h-14 rounded-xl flex items-center",
        active && !extended && "bg-primary "
      )}
    >
      {active && extended && (
        <span className="h-16 w-[6px] bg-primary absolute left-0 rounded-r-lg"></span>
      )}
      <Icon
        className={
          active ? (extended ? "fill-primary" : "fill-white") : "fill-disabled"
        }
      />
      <span
        className={twMerge(
          "w-fit transition-all overflow-hidden",
          !extended && "w-0"
        )}
      >
        {label}
      </span>
    </Link>
  );
}

export default MenuLink;
