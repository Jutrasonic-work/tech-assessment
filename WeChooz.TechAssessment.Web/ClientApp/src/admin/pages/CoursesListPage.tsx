import { Alert, Button, Group, Loader, Modal, Stack, Table, Text, Title } from "@mantine/core";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteCourse, getCourses } from "@/shared/api/adminCoursesApi";
import { ApiError } from "@/shared/api/http";
import type { GetCoursesItem } from "@/shared/api/types";
import { audienceLabel } from "@/shared/labels";

export function CoursesListPage() {
    const [rows, setRows] = useState<GetCoursesItem[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deleteTarget, setDeleteTarget] = useState<GetCoursesItem | null>(null);

    const load = () => {
        setLoading(true);
        setError(null);
        getCourses()
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
        await deleteCourse(deleteTarget.courseId);
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
            <Modal opened={!!deleteTarget} onClose={() => setDeleteTarget(null)} title="Supprimer la formation ?">
                <Text size="sm">
                    « {deleteTarget?.name} » sera supprimée définitivement.
                </Text>
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
                <Title order={2}>Formations</Title>
                <Button component={Link} to="/courses/new">
                    Nouvelle formation
                </Button>
            </Group>
            <Table striped highlightOnHover withTableBorder>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>Nom</Table.Th>
                        <Table.Th>Public</Table.Th>
                        <Table.Th>Durée (j)</Table.Th>
                        <Table.Th>Capacité</Table.Th>
                        <Table.Th w={200} />
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {rows?.map((c) => (
                        <Table.Tr key={c.courseId}>
                            <Table.Td>{c.name}</Table.Td>
                            <Table.Td>{audienceLabel(c.cseAudience)}</Table.Td>
                            <Table.Td>{c.durationDays}</Table.Td>
                            <Table.Td>{c.maxCapacity}</Table.Td>
                            <Table.Td>
                                <Group gap="xs" justify="flex-end">
                                    <Button component={Link} to={`/courses/${c.courseId}/edit`} size="xs" variant="light">
                                        Modifier
                                    </Button>
                                    <Button size="xs" color="red" variant="light" onClick={() => setDeleteTarget(c)}>
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
