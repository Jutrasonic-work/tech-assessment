import type { CseAudience, SessionDeliveryMode } from "@/shared/api/types";
import { CseAudience as A, SessionDeliveryMode as M } from "@/shared/api/types";

export function audienceLabel(a: CseAudience): string {
    switch (a) {
        case A.DelegateElu:
            return "Élu CSE";
        case A.President:
            return "Président de CSE";
        default:
            return String(a);
    }
}

export function deliveryLabel(m: SessionDeliveryMode): string {
    switch (m) {
        case M.InPerson:
            return "Présentiel";
        case M.Remote:
            return "À distance";
        default:
            return String(m);
    }
}

export function formatSessionStart(iso: string): string {
    return new Date(iso).toLocaleString("fr-FR", {
        dateStyle: "medium",
        timeStyle: "short",
    });
}
