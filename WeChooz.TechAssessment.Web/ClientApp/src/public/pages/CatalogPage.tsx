import {
    Alert,
    Badge,
    Button,
    Card,
    Grid,
    Group,
    Loader,
    Select,
    SimpleGrid,
    Stack,
    Text,
    TextInput,
    Title,
} from "@mantine/core";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getPublicSessions, type PublicSessionFilters } from "@/shared/api/publicSessionsApi";
import { ApiError } from "@/shared/api/http";
import type { CseAudience, GetPublicSessionsItem, SessionDeliveryMode } from "@/shared/api/types";
import { CseAudience as Aud, SessionDeliveryMode as Del } from "@/shared/api/types";
import { audienceLabel, deliveryLabel, formatSessionStart } from "@/shared/labels";

type DateFilterMode = "none" | "after" | "before" | "range";

export function CatalogPage() {
    const [items, setItems] = useState<GetPublicSessionsItem[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const [audience, setAudience] = useState<string | null>(null);
    const [deliveryMode, setDeliveryMode] = useState<string | null>(null);
    const [dateMode, setDateMode] = useState<DateFilterMode>("none");
    const [dateSingle, setDateSingle] = useState("");
    const [dateFrom, setDateFrom] = useState("");
    const [dateTo, setDateTo] = useState("");

    const load = () => {
        setLoading(true);
        setError(null);
        const f: PublicSessionFilters = {};
        if (audience !== null && audience !== "") {
            f.audience = Number(audience) as CseAudience;
        }
        if (deliveryMode !== null && deliveryMode !== "") {
            f.deliveryMode = Number(deliveryMode) as SessionDeliveryMode;
        }
        if (dateMode === "after" && dateSingle) {
            f.startAfter = new Date(dateSingle).toISOString();
        }
        if (dateMode === "before" && dateSingle) {
            f.startBefore = new Date(dateSingle).toISOString();
        }
        if (dateMode === "range" && dateFrom && dateTo) {
            f.startFrom = new Date(dateFrom).toISOString();
            f.startTo = new Date(dateTo).toISOString();
        }

        getPublicSessions(f)
            .then(setItems)
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? `${e.message} (${e.status})` : "Erreur réseau");
            })
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        load();
        // eslint-disable-next-line react-hooks/exhaustive-deps -- rechargement explicite via « Appliquer »
    }, []);

    return (
        <Stack gap="lg">
            <div>
                <Title order={1}>Sessions disponibles</Title>
                <Text c="dimmed" mt="xs">
                    Filtre par population cible, mode et dates de début.
                </Text>
            </div>

            <Card withBorder padding="lg" radius="md">
                <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }} spacing="md">
                    <Select
                        label="Population cible"
                        placeholder="Toutes"
                        clearable
                        data={[
                            { value: String(Aud.DelegateElu), label: audienceLabel(Aud.DelegateElu) },
                            { value: String(Aud.President), label: audienceLabel(Aud.President) },
                        ]}
                        value={audience}
                        onChange={setAudience}
                    />
                    <Select
                        label="Mode"
                        placeholder="Tous"
                        clearable
                        data={[
                            { value: String(Del.InPerson), label: deliveryLabel(Del.InPerson) },
                            { value: String(Del.Remote), label: deliveryLabel(Del.Remote) },
                        ]}
                        value={deliveryMode}
                        onChange={setDeliveryMode}
                    />
                    <Select
                        label="Filtre sur la date de début"
                        data={[
                            { value: "none", label: "Aucun" },
                            { value: "after", label: "Après une date" },
                            { value: "before", label: "Avant une date" },
                            { value: "range", label: "Entre deux dates" },
                        ]}
                        value={dateMode}
                        onChange={(v) => setDateMode((v as DateFilterMode) ?? "none")}
                    />
                </SimpleGrid>
                {dateMode === "after" || dateMode === "before" ? (
                    <TextInput
                        mt="md"
                        type="datetime-local"
                        label={dateMode === "after" ? "Début après" : "Début avant"}
                        value={dateSingle}
                        onChange={(e) => setDateSingle(e.currentTarget.value)}
                    />
                ) : null}
                {dateMode === "range" ? (
                    <SimpleGrid cols={{ base: 1, sm: 2 }} mt="md">
                        <TextInput type="datetime-local" label="Du" value={dateFrom} onChange={(e) => setDateFrom(e.currentTarget.value)} />
                        <TextInput type="datetime-local" label="Au" value={dateTo} onChange={(e) => setDateTo(e.currentTarget.value)} />
                    </SimpleGrid>
                ) : null}
                <Group justify="flex-end" mt="md">
                    <Button onClick={load}>Appliquer les filtres</Button>
                </Group>
            </Card>

            {error ? (
                <Alert color="red" title="Impossible de charger le catalogue">
                    {error}
                </Alert>
            ) : null}

            {loading ? (
                <Group justify="center" py="xl">
                    <Loader />
                </Group>
            ) : items && items.length === 0 ? (
                <Text c="dimmed">Aucune session ne correspond à ces critères.</Text>
            ) : (
                <Grid>
                    {items?.map((s) => (
                        <Grid.Col key={s.sessionId} span={{ base: 12, md: 6 }}>
                            <Card withBorder padding="lg" radius="md" h="100%">
                                <Stack gap="sm">
                                    <Group justify="space-between" align="flex-start">
                                        <Title order={4}>{s.courseName}</Title>
                                        <Badge variant="light">{deliveryLabel(s.deliveryMode)}</Badge>
                                    </Group>
                                    <Text size="sm" c="dimmed" lineClamp={3}>
                                        {s.shortDescription}
                                    </Text>
                                    <Group gap="xs">
                                        <Badge color="teal" variant="outline">
                                            {audienceLabel(s.cseAudience)}
                                        </Badge>
                                        <Text size="sm" c="dimmed">
                                            {formatSessionStart(s.startDate)} · {s.durationDays} j
                                        </Text>
                                    </Group>
                                    <Text size="sm">
                                        <Text span fw={500}>
                                            Places restantes :
                                        </Text>{" "}
                                        {s.remainingSeats}
                                    </Text>
                                    <Text size="sm" c="dimmed">
                                        {s.trainerFirstName} {s.trainerLastName}
                                    </Text>
                                    <Button component={Link} to={`/session/${s.sessionId}`} variant="light" fullWidth mt="auto">
                                        Voir le détail
                                    </Button>
                                </Stack>
                            </Card>
                        </Grid.Col>
                    ))}
                </Grid>
            )}
        </Stack>
    );
}
