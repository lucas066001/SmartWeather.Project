import { Toaster } from "@/components/ui/toaster"

export default function RootLayout({ children }) {
  return (
    <>
      <html lang="fr">
        <body>
          {children}
          <Toaster />
        </body>
      </html>
    </>
  )
}
