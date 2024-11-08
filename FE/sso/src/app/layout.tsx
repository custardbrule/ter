"use client";
import "@/public/assets/css/globals.css";
import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import StoreProvider from "@/lib/providers/store-provider";
import AppThemeProvider from "@/lib/providers/app-theme-provider";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head>
        <link rel="icon" href="/favicon.ico" />
        <meta charSet="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
      </head>
      <StoreProvider>
        <AppThemeProvider>
          <body>{children}</body>
        </AppThemeProvider>
      </StoreProvider>
    </html>
  );
}
