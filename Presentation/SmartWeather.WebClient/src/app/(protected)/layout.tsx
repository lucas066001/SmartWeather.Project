import type { Metadata } from "next";
import "@/app/globals.css";
import SideBarMenu from "@/components/ui/SideBarMenu";
import outfitFonts from "@/app/fonts/Outfit";
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
      <head>
        <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
        <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
      </head>
      <body className={`${outfitFonts.variable} antialiased flex bg-slate-50`}>
        <SideBarMenu menu={menu} />
        <div className="flex-1">
          {children}
        </div>
      </body>
    </html>
  );
}
