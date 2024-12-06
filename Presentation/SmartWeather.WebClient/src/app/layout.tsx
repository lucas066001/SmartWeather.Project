import type { Metadata } from "next";
import "./globals.css";
import SideBarMenu from "@/components/ui/SideBarMenu";
import outfitFonts from "./fonts/Outfit";
import Header from "@/components/ui/Header";
import menu from "@/conf/menu";

export const metadata: Metadata = {
  title: "SwartWeather",
  description: "SmartWeather App",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="fr">
      <body className={`${outfitFonts.variable} antialiased flex bg-slate-50`}>
        <SideBarMenu menu={menu}/>
        <div className="flex-1">
          {children}
        </div>
      </body>
    </html>
  );
}
