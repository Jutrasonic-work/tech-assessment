import type { LoginResponse } from "@/shared/api/types";
import { apiJson } from "@/shared/api/http";

export function login(loginName: string): Promise<LoginResponse> {
    return apiJson<LoginResponse>("/api/auth/login", {
        method: "POST",
        body: JSON.stringify({ login: loginName }),
    });
}
