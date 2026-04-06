const STORAGE_KEY = "wechooz_admin_login";

export type AdminRole = "formation" | "sales";

export function getStoredRole(): AdminRole | null {
    const v = sessionStorage.getItem(STORAGE_KEY);
    if (v === "formation" || v === "sales") {
        return v;
    }
    return null;
}

export function setStoredRole(role: AdminRole): void {
    sessionStorage.setItem(STORAGE_KEY, role);
}

export function clearStoredRole(): void {
    sessionStorage.removeItem(STORAGE_KEY);
}

export function parseLoginRole(login: string): AdminRole | null {
    const l = login.trim().toLowerCase();
    if (l === "formation" || l === "sales") {
        return l;
    }
    return null;
}
