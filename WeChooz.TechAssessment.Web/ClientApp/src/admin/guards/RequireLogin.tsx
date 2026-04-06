import { Navigate, Outlet } from "react-router-dom";
import { getStoredRole } from "@/admin/auth/adminSession";

export function RequireLogin() {
    if (!getStoredRole()) {
        return <Navigate to="/login" replace />;
    }
    return <Outlet />;
}
