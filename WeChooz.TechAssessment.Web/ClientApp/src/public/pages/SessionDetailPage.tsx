import { Alert, Anchor, Badge, Button, Group, Loader, Paper, Stack, Text, Title } from "@mantine/core";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getPublicSessionDetail } from "@/shared/api/publicSessionsApi";
import { ApiError } from "@/shared/api/http";
import type { GetPublicSessionDetailResponse } from "@/shared/api/types";
import { audienceLabel, deliveryLabel, formatSessionStart } from "@/shared/labels";

export function SessionDetailPage() {
    const { sessionId } = useParams<{ sessionId: string }>();
    const id = Number(sessionId);
    const [data, setData] = useState<GetPublicSessionDetailResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!Number.isFinite(id)) {
            setError("Identifiant invalide");
            setLoading(false);
            return;
        }
        setLoading(true);
        getPublicSessionDetail(id)
            .then(setData)
            .catch((e: unknown) => {
                if (e instanceof ApiError && e.status === 404) {
                    setError("Session introuvable.");
                } else {
                    setError(e instanceof ApiError ? e.message : "Erreur réseau");
                }
            })
            .finally(() => setLoading(false));
    }, [id]);

    if (loading) {
        return (
            <Group justify="center" py="xl">
                <Loader />
            </Group>
        );
    }

    if (error || !data) {
        return (
            <Stack>
                <Alert color="red" title="Erreur">
                    {error ?? "Données indisponibles"}
                </Alert>
                <Anchor component={Link} to="/">
                    Retour au catalogue
                </Anchor>
            </Stack>
        );
    }

    return (
        <Stack gap="lg">
            <Button component={Link} to="/" variant="subtle" size="xs" w="fit-content" px={0}>
                ← Catalogue
            </Button>
            <div>
                <Group gap="sm" mb="xs">
                    <Badge variant="light">{deliveryLabel(data.deliveryMode)}</Badge>
                    <Badge color="teal" variant="outline">
                        {audienceLabel(data.cseAudience)}
                    </Badge>
                </Group>
                <Title order={1}>{data.courseName}</Title>
                <Text c="dimmed" mt="xs">
                    {formatSessionStart(data.startDate)} · {data.durationDays} jour{data.durationDays > 1 ? "s" : ""} · {data.remainingSeats}{" "}
                    place{data.remainingSeats > 1 ? "s" : ""} restante{data.remainingSeats > 1 ? "s" : ""}
                </Text>
            </div>

            <Paper withBorder p="md" radius="md">
                <Text fw={500} mb="xs">
                    Chapô
                </Text>
                <Text size="sm" c="dimmed">
                    {data.shortDescription}
                </Text>
            </Paper>

            <Paper withBorder p="md" radius="md">
                <Text fw={500} mb="xs">
                    Formateur
                </Text>
                <Text size="sm">
                    {data.trainerFirstName} {data.trainerLastName}
                </Text>
            </Paper>

            <Paper withBorder p="md" radius="md">
                <Text fw={500} mb="md">
                    Programme
                </Text>
                <div
                    className="session-long-desc"
                    dangerouslySetInnerHTML={{ __html: data.longDescriptionHtml }}
                    style={{ fontSize: "var(--mantine-font-size-sm)", lineHeight: 1.65 }}
                />
            </Paper>
        </Stack>
    );
}
