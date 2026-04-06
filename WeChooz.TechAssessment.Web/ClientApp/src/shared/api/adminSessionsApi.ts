import { apiJson } from "@/shared/api/http";
import type {
    CreateSessionBody,
    CreateSessionResponse,
    GetAdminSessionByIdResponse,
    GetAdminSessionsItem,
    UpdateSessionBody,
} from "@/shared/api/types";

export function getAdminSessions(): Promise<GetAdminSessionsItem[]> {
    return apiJson<GetAdminSessionsItem[]>("/api/admin/sessions");
}

export function getAdminSessionById(sessionId: number): Promise<GetAdminSessionByIdResponse> {
    return apiJson<GetAdminSessionByIdResponse>(`/api/admin/sessions/${sessionId}`);
}

export function createSession(body: CreateSessionBody): Promise<CreateSessionResponse> {
    return apiJson<CreateSessionResponse>("/api/admin/sessions", {
        method: "POST",
        body: JSON.stringify(body),
    });
}

export function updateSession(sessionId: number, body: UpdateSessionBody): Promise<void> {
    return apiJson<void>(`/api/admin/sessions/${sessionId}`, {
        method: "PUT",
        body: JSON.stringify(body),
    });
}

export function deleteSession(sessionId: number): Promise<void> {
    return apiJson<void>(`/api/admin/sessions/${sessionId}`, {
        method: "DELETE",
    });
}
