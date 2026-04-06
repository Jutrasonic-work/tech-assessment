import { Alert, Button, Group, Loader, Paper, Stack, Text, Title } from "@mantine/core";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getAdminSessionById } from "@/shared/api/adminSessionsApi";
import { ApiError } from "@/shared/api/http";
import type { GetAdminSessionByIdResponse } from "@/shared/api/types";
import { deliveryLabel, formatSessionStart } from "@/shared/labels";

export function AdminSessionDetailPage() {
    const { sessionId } = useParams<{ sessionId: string }>();
    const id = Number(sessionId);
    const [data, setData] = useState<GetAdminSessionByIdResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!Number.isFinite(id)) {
            setError("ID invalide");
            setLoading(false);
            return;
        }
        getAdminSessionById(id)
            .then(setData)
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? e.message : "Erreur");
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
        return <Alert color="red">{error ?? "Introuvable"}</Alert>;
    }

    return (
        <Stack gap="md" maw={640}>
            <Button component={Link} to="/sessions" variant="subtle" size="xs" w="fit-content" px={0}>
                ← Sessions
            </Button>
            <Title order={2}>{data.courseName}</Title>
            <Paper withBorder p="md" radius="md">
                <Stack gap="xs">
                    <Text>
                        <Text span fw={600}>
                            Début :
                        </Text>{" "}
                        {formatSessionStart(data.startDate)}
                    </Text>
                    <Text>
                        <Text span fw={600}>
                            Mode :
                        </Text>{" "}
                        {deliveryLabel(data.deliveryMode)}
                    </Text>
                    <Text>
                        <Text span fw={600}>
                            Participants :
                        </Text>{" "}
                        {data.participantCount} / {data.maxCapacity}
                    </Text>
                </Stack>
            </Paper>
            <Group>
                <Button component={Link} to={`/sessions/${data.sessionId}/participants`}>
                    Gérer les participants
                </Button>
                <Button component={Link} to={`/sessions/${data.sessionId}/edit`} variant="light">
                    Modifier
                </Button>
            </Group>
        </Stack>
    );
}
