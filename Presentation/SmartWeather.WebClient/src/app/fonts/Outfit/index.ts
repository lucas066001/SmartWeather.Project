import localFont from "next/font/local";

const outfitFonts = localFont({
    src: [
      {
        path: "./static/Outfit-Thin.ttf",
        weight: "100",
        style: "normal",
      },
      {
        path: "./static/Outfit-ExtraLight.ttf",
        weight: "200",
        style: "normal",
      },
      {
        path: "./static/Outfit-Light.ttf",
        weight: "300",
        style: "normal",
      },
      {
        path: "./static/Outfit-Regular.ttf",
        weight: "400",
        style: "normal",
      },
      {
        path: "./static/Outfit-Medium.ttf",
        weight: "500",
        style: "normal",
      },
      {
        path: "./static/Outfit-SemiBold.ttf",
        weight: "600",
        style: "normal",
      },
      {
        path: "./static/Outfit-Bold.ttf",
        weight: "700",
        style: "normal",
      },
      {
        path: "./static/Outfit-ExtraBold.ttf",
        weight: "800",
        style: "normal",
      },
      {
        path: "./static/Outfit-Black.ttf",
        weight: "900",
        style: "normal",
      },
    ],
    variable: "--font-outfit",
  });

  export default outfitFonts