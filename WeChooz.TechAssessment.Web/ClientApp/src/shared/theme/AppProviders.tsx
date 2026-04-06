import "@mantine/core/styles.css";

import { createTheme, localStorageColorSchemeManager, MantineProvider } from "@mantine/core";
import type { ReactNode } from "react";

const colorSchemeManager = localStorageColorSchemeManager({ key: "wechooz-color-scheme" });

const theme = createTheme({
    primaryColor: "teal",
    fontFamily: "system-ui, -apple-system, Segoe UI, Roboto, sans-serif",
    defaultRadius: "md",
    headings: { fontWeight: "600" },
});

export function AppProviders({ children }: { children: ReactNode }) {
    return (
        <MantineProvider theme={theme} colorSchemeManager={colorSchemeManager} defaultColorScheme="auto">
            {children}
        </MantineProvider>
    );
}
