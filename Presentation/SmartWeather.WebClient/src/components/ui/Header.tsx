"use client"
import UserMenu from "./userMenu";

export interface IHeaderProps {
  title?: string;
}

function Header({ title }: IHeaderProps) {
  return (
    <header className="w-full border-b h-20 bg-white flex items-center justify-between">
      {title && (
        <h1 className="font-semibold text-titleBrown text-3xl mx-auto">{title}</h1>
      )}
        <UserMenu className="mr-4"></UserMenu>
    </header>
  );
}

export default Header;
