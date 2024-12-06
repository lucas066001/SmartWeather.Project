import type { Metadata } from "next";
import "../globals.css";
import outfitFonts from "../fonts/Outfit";
import logoExtended from "@/../public/images/logo-extended.svg";
import Image from "next/image"

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
      <body className={`${outfitFonts.variable} antialiased flex flex-col min-h-screen justify-between items-center p-6 bg-slate-50`}>
        <Image
          src={logoExtended}
          alt="smartweather-logo"
        />
        <div className="mt-6 flex flex-col items-center p-3 gap-3 min-w-80  min-h-80 bg-mainBackground rounded-md shadow-md">
          {children}
        </div>
        <div className="px-6 py-4 ">
          <p className="text-xs text-center text-[#e5e7eb]">
            © 2024 SmartWeather, all right reserved
          </p>
        </div>
      </body>
    </html>
  );
}
