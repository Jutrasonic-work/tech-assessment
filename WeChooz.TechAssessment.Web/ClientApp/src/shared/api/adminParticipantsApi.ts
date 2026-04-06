import { apiJson } from "@/shared/api/http";
import type { AddParticipantBody, AddParticipantResponse, GetParticipantsBySessionItem, UpdateParticipantBody } from "@/shared/api/types";

export function getParticipants(sessionId: number): Promise<GetParticipantsBySessionItem[]> {
    return apiJson<GetParticipantsBySessionItem[]>(`/api/admin/sessions/${sessionId}/participants`);
}

export function addParticipant(sessionId: number, body: AddParticipantBody): Promise<AddParticipantResponse> {
    return apiJson<AddParticipantResponse>(`/api/admin/sessions/${sessionId}/participants`, {
        method: "POST",
        body: JSON.stringify(body),
    });
}

export function updateParticipant(participantId: number, body: UpdateParticipantBody): Promise<void> {
    return apiJson<void>(`/api/admin/participants/${participantId}`, {
        method: "PUT",
        body: JSON.stringify(body),
    });
}

export function deleteParticipant(participantId: number): Promise<void> {
    return apiJson<void>(`/api/admin/participants/${participantId}`, {
        method: "DELETE",
    });
}
