import { Alert, Button, Group, Loader, Modal, Stack, Table, Text, Title } from "@mantine/core";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteSession, getAdminSessions } from "@/shared/api/adminSessionsApi";
import { ApiError } from "@/shared/api/http";
import type { GetAdminSessionsItem } from "@/shared/api/types";
import { deliveryLabel, formatSessionStart } from "@/shared/labels";

export function SessionsListPage() {
    const [rows, setRows] = useState<GetAdminSessionsItem[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deleteTarget, setDeleteTarget] = useState<GetAdminSessionsItem | null>(null);

    const load = () => {
        setLoading(true);
        setError(null);
        getAdminSessions()
            .then(setRows)
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? `${e.message} (${e.status})` : "Erreur réseau");
            })
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        load();
    }, []);

    const runDelete = async () => {
        if (!deleteTarget) {
            return;
        }
        await deleteSession(deleteTarget.sessionId);
        setDeleteTarget(null);
        load();
    };

    if (loading) {
        return (
            <Group justify="center" py="xl">
                <Loader />
            </Group>
        );
    }

    if (error) {
        return (
            <Alert color="red" title="Erreur">
                {error}
            </Alert>
        );
    }

    return (
        <Stack gap="md">
            <Modal opened={!!deleteTarget} onClose={() => setDeleteTarget(null)} title="Supprimer la session ?">
                <Text size="sm">La session du {deleteTarget ? formatSessionStart(deleteTarget.startDate) : ""} sera supprimée.</Text>
                <Group justify="flex-end" mt="md">
                    <Button variant="default" onClick={() => setDeleteTarget(null)}>
                        Annuler
                    </Button>
                    <Button color="red" onClick={runDelete}>
                        Supprimer
                    </Button>
                </Group>
            </Modal>
            <Group justify="space-between" align="flex-end">
                <Title order={2}>Sessions</Title>
                <Button component={Link} to="/sessions/new">
                    Nouvelle session
                </Button>
            </Group>
            <Table striped highlightOnHover withTableBorder>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>Formation</Table.Th>
                        <Table.Th>Début</Table.Th>
                        <Table.Th>Mode</Table.Th>
                        <Table.Th>Participants</Table.Th>
                        <Table.Th miw={420} style={{ width: "1%" }} />
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {rows?.map((s) => (
                        <Table.Tr key={s.sessionId}>
                            <Table.Td>{s.courseName}</Table.Td>
                            <Table.Td>{formatSessionStart(s.startDate)}</Table.Td>
                            <Table.Td>{deliveryLabel(s.deliveryMode)}</Table.Td>
                            <Table.Td>
                                {s.participantCount} / {s.maxCapacity}
                            </Table.Td>
                            <Table.Td>
                                <Group gap="xs" justify="flex-end" wrap="nowrap">
                                    <Button component={Link} to={`/sessions/${s.sessionId}`} size="xs" variant="light">
                                        Détail
                                    </Button>
                                    <Button component={Link} to={`/sessions/${s.sessionId}/participants`} size="xs" variant="light">
                                        Participants
                                    </Button>
                                    <Button component={Link} to={`/sessions/${s.sessionId}/edit`} size="xs" variant="default">
                                        Modifier
                                    </Button>
                                    <Button size="xs" color="red" variant="light" onClick={() => setDeleteTarget(s)}>
                                        Supprimer
                                    </Button>
                                </Group>
                            </Table.Td>
                        </Table.Tr>
                    ))}
                </Table.Tbody>
            </Table>
        </Stack>
    );
}
