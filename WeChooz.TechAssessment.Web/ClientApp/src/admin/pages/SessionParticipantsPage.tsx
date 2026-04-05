import {
    Alert,
    Button,
    Group,
    Loader,
    Modal,
    Stack,
    Table,
    Text,
    TextInput,
    Title,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import {
    addParticipant,
    deleteParticipant,
    getParticipants,
    updateParticipant,
} from "@/shared/api/adminParticipantsApi";
import { getAdminSessionById } from "@/shared/api/adminSessionsApi";
import { ApiError } from "@/shared/api/http";
import type { GetParticipantsBySessionItem } from "@/shared/api/types";
import { formatSessionStart } from "@/shared/labels";
import { getStoredRole } from "@/admin/auth/adminSession";

type PForm = {
    lastName: string;
    firstName: string;
    email: string;
    companyName: string;
};

export function SessionParticipantsPage() {
    const { sessionId } = useParams<{ sessionId: string }>();
    const sid = Number(sessionId);
    const [sessionTitle, setSessionTitle] = useState<string | null>(null);
    const [rows, setRows] = useState<GetParticipantsBySessionItem[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [addOpen, setAddOpen] = useState(false);
    const [editRow, setEditRow] = useState<GetParticipantsBySessionItem | null>(null);
    const [deleteRow, setDeleteRow] = useState<GetParticipantsBySessionItem | null>(null);

    const load = () => {
        if (!Number.isFinite(sid)) {
            return;
        }
        setLoading(true);
        setError(null);
        Promise.all([
            getParticipants(sid),
            getStoredRole() === "formation" ? getAdminSessionById(sid).catch(() => null) : Promise.resolve(null),
        ])
            .then(([parts, meta]) => {
                setRows(parts);
                if (meta) {
                    setSessionTitle(`${meta.courseName} · ${formatSessionStart(meta.startDate)}`);
                }
            })
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? `${e.message} (${e.status})` : "Erreur réseau");
            })
            .finally(() => setLoading(false));
    };

    useEffect(() => {
        load();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [sid]);

    const pValidate = {
        lastName: (v: string) => (!v?.trim() ? "Requis" : null),
        firstName: (v: string) => (!v?.trim() ? "Requis" : null),
        email: (v: string) => (!v?.trim() ? "Requis" : null),
        companyName: (v: string) => (!v?.trim() ? "Requis" : null),
    };

    const addForm = useForm<PForm>({
        initialValues: { lastName: "", firstName: "", email: "", companyName: "" },
        validate: pValidate,
    });

    const editForm = useForm<PForm>({
        initialValues: { lastName: "", firstName: "", email: "", companyName: "" },
        validate: pValidate,
    });

    useEffect(() => {
        if (editRow) {
            editForm.setValues({
                lastName: editRow.lastName,
                firstName: editRow.firstName,
                email: editRow.email,
                companyName: editRow.companyName,
            });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [editRow]);

    const submitAdd = addForm.onSubmit(async (v) => {
        await addParticipant(sid, {
            lastName: v.lastName.trim(),
            firstName: v.firstName.trim(),
            email: v.email.trim(),
            companyName: v.companyName.trim(),
        });
        setAddOpen(false);
        addForm.reset();
        load();
    });

    const submitEdit = editForm.onSubmit(async (v) => {
        if (!editRow) {
            return;
        }
        await updateParticipant(editRow.participantId, {
            sessionId: sid,
            lastName: v.lastName.trim(),
            firstName: v.firstName.trim(),
            email: v.email.trim(),
            companyName: v.companyName.trim(),
        });
        setEditRow(null);
        load();
    });

    const runDelete = async () => {
        if (!deleteRow) {
            return;
        }
        await deleteParticipant(deleteRow.participantId);
        setDeleteRow(null);
        load();
    };

    if (!Number.isFinite(sid)) {
        return <Alert color="red">Identifiant de session invalide.</Alert>;
    }

    if (loading && !rows) {
        return (
            <Group justify="center" py="xl">
                <Loader />
            </Group>
        );
    }

    return (
        <Stack gap="md">
            <Modal opened={addOpen} onClose={() => setAddOpen(false)} title="Ajouter un participant">
                <form onSubmit={submitAdd}>
                    <Stack gap="sm">
                        <TextInput label="Nom" required {...addForm.getInputProps("lastName")} />
                        <TextInput label="Prénom" required {...addForm.getInputProps("firstName")} />
                        <TextInput label="E-mail" required type="email" {...addForm.getInputProps("email")} />
                        <TextInput label="Entreprise" required {...addForm.getInputProps("companyName")} />
                        <Group justify="flex-end" mt="sm">
                            <Button variant="default" type="button" onClick={() => setAddOpen(false)}>
                                Annuler
                            </Button>
                            <Button type="submit">Ajouter</Button>
                        </Group>
                    </Stack>
                </form>
            </Modal>

            <Modal opened={!!editRow} onClose={() => setEditRow(null)} title="Modifier le participant">
                <form onSubmit={submitEdit}>
                    <Stack gap="sm">
                        <TextInput label="Nom" required {...editForm.getInputProps("lastName")} />
                        <TextInput label="Prénom" required {...editForm.getInputProps("firstName")} />
                        <TextInput label="E-mail" required type="email" {...editForm.getInputProps("email")} />
                        <TextInput label="Entreprise" required {...editForm.getInputProps("companyName")} />
                        <Group justify="flex-end" mt="sm">
                            <Button variant="default" type="button" onClick={() => setEditRow(null)}>
                                Annuler
                            </Button>
                            <Button type="submit">Enregistrer</Button>
                        </Group>
                    </Stack>
                </form>
            </Modal>

            <Modal opened={!!deleteRow} onClose={() => setDeleteRow(null)} title="Retirer ce participant ?">
                <Text size="sm">
                    {deleteRow?.firstName} {deleteRow?.lastName}
                </Text>
                <Group justify="flex-end" mt="md">
                    <Button variant="default" onClick={() => setDeleteRow(null)}>
                        Annuler
                    </Button>
                    <Button color="red" onClick={runDelete}>
                        Supprimer
                    </Button>
                </Group>
            </Modal>

            <Button
                component={Link}
                to={getStoredRole() === "formation" ? "/sessions" : "/sales-access"}
                variant="subtle"
                size="xs"
                w="fit-content"
                px={0}
            >
                ← Retour
            </Button>
            <Group justify="space-between" align="flex-end">
                <div>
                    <Title order={2}>Participants</Title>
                    {sessionTitle ? (
                        <Text size="sm" c="dimmed" mt={4}>
                            {sessionTitle}
                        </Text>
                    ) : null}
                </div>
                <Button onClick={() => setAddOpen(true)}>Ajouter</Button>
            </Group>

            {error ? (
                <Alert color="red" title="Erreur">
                    {error}
                </Alert>
            ) : null}

            <Table striped highlightOnHover withTableBorder>
                <Table.Thead>
                    <Table.Tr>
                        <Table.Th>Nom</Table.Th>
                        <Table.Th>Prénom</Table.Th>
                        <Table.Th>E-mail</Table.Th>
                        <Table.Th>Entreprise</Table.Th>
                        <Table.Th w={160} />
                    </Table.Tr>
                </Table.Thead>
                <Table.Tbody>
                    {rows?.map((p) => (
                        <Table.Tr key={p.participantId}>
                            <Table.Td>{p.lastName}</Table.Td>
                            <Table.Td>{p.firstName}</Table.Td>
                            <Table.Td>{p.email}</Table.Td>
                            <Table.Td>{p.companyName}</Table.Td>
                            <Table.Td>
                                <Group gap="xs" justify="flex-end">
                                    <Button size="xs" variant="light" onClick={() => setEditRow(p)}>
                                        Modifier
                                    </Button>
                                    <Button size="xs" color="red" variant="light" onClick={() => setDeleteRow(p)}>
                                        Retirer
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
