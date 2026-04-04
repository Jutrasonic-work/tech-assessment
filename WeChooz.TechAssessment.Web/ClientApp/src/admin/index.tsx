import { createRoot } from "react-dom/client";

const container = document.getElementById("react-app");
if (!container) {
    throw new Error("Root element #react-app not found");
}

const root = createRoot(container);
root.render(<h1>Hello admin page</h1>);
