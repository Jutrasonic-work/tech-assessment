import { apiJson, buildQuery } from "@/shared/api/http";
import type { CseAudience, GetPublicSessionDetailResponse, GetPublicSessionsItem, SessionDeliveryMode } from "@/shared/api/types";

export type PublicSessionFilters = {
    audience?: CseAudience | null;
    deliveryMode?: SessionDeliveryMode | null;
    startAfter?: string | null;
    startBefore?: string | null;
    startFrom?: string | null;
    startTo?: string | null;
};

export function getPublicSessions(filters: PublicSessionFilters): Promise<GetPublicSessionsItem[]> {
    const q = buildQuery({
        audience: filters.audience ?? undefined,
        deliveryMode: filters.deliveryMode ?? undefined,
        startAfter: filters.startAfter ?? undefined,
        startBefore: filters.startBefore ?? undefined,
        startFrom: filters.startFrom ?? undefined,
        startTo: filters.startTo ?? undefined,
    });
    return apiJson<GetPublicSessionsItem[]>(`/api/public/sessions${q}`);
}

export function getPublicSessionDetail(sessionId: number): Promise<GetPublicSessionDetailResponse> {
    return apiJson<GetPublicSessionDetailResponse>(`/api/public/sessions/${sessionId}`);
}
