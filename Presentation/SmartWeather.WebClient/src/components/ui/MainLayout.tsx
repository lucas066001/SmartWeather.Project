import React, { PropsWithChildren } from "react";
import Header from "./Header";

interface IMainLayoutProps extends PropsWithChildren {
  title?: string;
}

function MainLayout({ title, children }: IMainLayoutProps) {
  return (
    <>
      <Header title={title} />
      <main className="w-full p-7">{children}</main>
    </>
  );
}

export default MainLayout;
