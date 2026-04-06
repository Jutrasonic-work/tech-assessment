export class ApiError extends Error {
    readonly status: number;
    readonly bodyText?: string;

    constructor(message: string, status: number, bodyText?: string) {
        super(message);
        this.name = "ApiError";
        this.status = status;
        this.bodyText = bodyText;
    }
}

const defaultHeaders: HeadersInit = {
    "Content-Type": "application/json",
};

export async function apiJson<T>(path: string, init?: RequestInit): Promise<T> {
    const res = await fetch(path, {
        ...init,
        credentials: "include",
        headers: { ...defaultHeaders, ...init?.headers },
    });

    const text = await res.text();
    if (!res.ok) {
        throw new ApiError(res.statusText || `HTTP ${res.status}`, res.status, text || undefined);
    }

    if (!text) {
        return undefined as T;
    }

    return JSON.parse(text) as T;
}

export function buildQuery(params: Record<string, string | number | undefined | null>): string {
    const usp = new URLSearchParams();
    for (const [k, v] of Object.entries(params)) {
        if (v === undefined || v === null || v === "") {
            continue;
        }
        usp.set(k, String(v));
    }
    const s = usp.toString();
    return s ? `?${s}` : "";
}
