import type { Config } from "tailwindcss";

export default {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "#67C44D",
        secondary: "#604538",
        disabled: "#B1B1B1",
        titleBrown: "#745427",
        textGreen: "#468534",
        alert: "#D9564D",
        warning: "#E1AC49",
        textWarning: "#C99E4E",
        textAlert: "#C35750",
        mainBackground: "#FCFCFC",
        blue: "#5384AF",
        textBlue: "#144D6C"
      },
      fontFamily: {
        outfit: ['var(--font-outfit)', 'sans-serif'],
      },
    },
  },
  plugins: [],
} satisfies Config;
