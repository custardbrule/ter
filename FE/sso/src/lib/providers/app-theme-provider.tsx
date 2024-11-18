"use client";

import { useState, useEffect } from "react";
import { ThemeProvider } from "@emotion/react";
import { createTheme, CssBaseline, PaletteMode } from "@mui/material";
import { ToastContainer } from "react-toastify";

export default function AppThemeProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [theme, setTheme] = useState<PaletteMode>("light");

  useEffect(() => {
    const isDark =
      window.matchMedia &&
      window.matchMedia("(prefers-color-scheme: dark)").matches;

    setTheme(isDark ? "dark" : "light");
  }, []);

  useEffect(() => {
    const handler = (event: MediaQueryListEvent) => {
      setTheme(event.matches ? "dark" : "light");
    };
    window
      .matchMedia("(prefers-color-scheme: dark)")
      .addEventListener("change", handler);

    return () => {
      window
        .matchMedia("(prefers-color-scheme: dark)")
        .removeEventListener("change", handler);
    };
  }, []);

  const getTheme = () =>
    createTheme({
      palette: {
        mode: theme,
      },
    });

  return (
    <ThemeProvider theme={getTheme}>
      <ToastContainer
        theme={theme}
        position="top-right"
        autoClose={5000}
        newestOnTop={false}
        closeOnClick
      />
      <CssBaseline />
      {children}
    </ThemeProvider>
  );
}
