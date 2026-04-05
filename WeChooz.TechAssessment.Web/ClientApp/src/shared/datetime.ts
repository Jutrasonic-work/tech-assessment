/** Pour `input type="datetime-local"` (heure locale). */
export function isoToDatetimeLocal(iso: string): string {
    const d = new Date(iso);
    const p = (n: number) => n.toString().padStart(2, "0");
    return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}T${p(d.getHours())}:${p(d.getMinutes())}`;
}

export function datetimeLocalToIso(local: string): string {
    return new Date(local).toISOString();
}
