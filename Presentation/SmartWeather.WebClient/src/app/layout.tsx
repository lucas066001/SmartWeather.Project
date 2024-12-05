import type { Metadata } from "next";
import localFont from "next/font/local";
import "./globals.css";

const outfitFonts = localFont({
  src: [
    {
      path: "./fonts/Outfit/static/Outfit-Thin.ttf",
      weight: "100",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-ExtraLight.ttf",
      weight: "200",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-Light.ttf",
      weight: "300",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-Regular.ttf",
      weight: "400",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-Medium.ttf",
      weight: "500",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-SemiBold.ttf",
      weight: "600",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-Bold.ttf",
      weight: "700",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-ExtraBold.ttf",
      weight: "800",
      style: "normal",
    },
    {
      path: "./fonts/Outfit/static/Outfit-Black.ttf",
      weight: "900",
      style: "normal",
    },
  ],
  variable: "--font-outfit",
});


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
    <html lang="en">
      <body
        className={`${outfitFonts.variable} antialiased`}
      >
        {children}
      </body>
    </html>
  );
}
