import { Navigate, Outlet } from "react-router-dom";
import { getStoredRole } from "@/admin/auth/adminSession";

export function FormationOnly() {
    if (getStoredRole() !== "formation") {
        return <Navigate to="/sales-access" replace />;
    }
    return <Outlet />;
}
