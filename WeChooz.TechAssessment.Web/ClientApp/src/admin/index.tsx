import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { AdminApp } from "@/admin/AdminApp";

const container = document.getElementById("react-app");
if (!container) {
    throw new Error("Root element #react-app not found");
}

createRoot(container).render(
    <StrictMode>
        <AdminApp />
    </StrictMode>,
);
