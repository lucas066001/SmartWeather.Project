import outfitFonts from "@/app/fonts/Outfit";
import "@/app/globals.css";
import SideBarMenu from "@/components/ui/sideBarMenu";
import menu from "@/conf/menu";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "SwartWeather",
  description: "SmartWeather App",
};

export default function ProtectedLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <div
      className={`${outfitFonts.variable} antialiased flex bg-slate-50 font-outfit`}
    >
      <SideBarMenu menu={menu} />
      <div className="flex-1">{children}</div>
    </div>
  );
}
