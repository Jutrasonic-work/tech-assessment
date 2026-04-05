import { Navigate } from "react-router-dom";
import { getStoredRole } from "@/admin/auth/adminSession";

export function AdminStart() {
    const r = getStoredRole();
    if (r === "formation") {
        return <Navigate to="/courses" replace />;
    }
    return <Navigate to="/sales-access" replace />;
}
