import { Alert, Button, Group, Loader, Select, Stack, TextInput, Title } from "@mantine/core";
import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { createSession, getAdminSessionById, updateSession } from "@/shared/api/adminSessionsApi";
import { getCourses } from "@/shared/api/adminCoursesApi";
import { ApiError } from "@/shared/api/http";
import type { GetCoursesItem } from "@/shared/api/types";
import type { SessionDeliveryMode } from "@/shared/api/types";
import { SessionDeliveryMode as Del } from "@/shared/api/types";
import { datetimeLocalToIso, isoToDatetimeLocal } from "@/shared/datetime";
import { deliveryLabel } from "@/shared/labels";

type FormValues = {
    courseId: string;
    startDateLocal: string;
    deliveryMode: string;
};

export function SessionEditPage() {
    const { sessionId } = useParams<{ sessionId: string }>();
    const isNew = sessionId === undefined;
    const navigate = useNavigate();
    const [courses, setCourses] = useState<GetCoursesItem[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<FormValues>({
        initialValues: {
            courseId: "",
            startDateLocal: "",
            deliveryMode: String(Del.InPerson),
        },
        validate: {
            courseId: (v) => (!v ? "Choisis une formation" : null),
            startDateLocal: (v) => (!v ? "Date requise" : null),
        },
    });

    useEffect(() => {
        let cancelled = false;
        (async () => {
            try {
                const list = await getCourses();
                if (cancelled) {
                    return;
                }
                setCourses(list);
                if (isNew && list.length > 0 && !form.values.courseId) {
                    form.setFieldValue("courseId", String(list[0].courseId));
                }
            } catch (e: unknown) {
                if (!cancelled) {
                    setError(e instanceof ApiError ? e.message : "Impossible de charger les formations");
                }
            }
        })();
        return () => {
            cancelled = true;
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isNew]);

    useEffect(() => {
        if (isNew || !sessionId) {
            setLoading(false);
            return;
        }
        const id = Number(sessionId);
        setLoading(true);
        getAdminSessionById(id)
            .then((s) => {
                form.setValues({
                    courseId: String(s.courseId),
                    startDateLocal: isoToDatetimeLocal(s.startDate),
                    deliveryMode: String(s.deliveryMode),
                });
            })
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? e.message : "Erreur chargement");
            })
            .finally(() => setLoading(false));
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [sessionId, isNew]);

    const submit = form.onSubmit(async (values) => {
        setError(null);
        const body = {
            courseId: Number(values.courseId),
            startDate: datetimeLocalToIso(values.startDateLocal),
            deliveryMode: Number(values.deliveryMode) as SessionDeliveryMode,
        };
        try {
            if (isNew) {
                await createSession(body);
            } else {
                await updateSession(Number(sessionId), body);
            }
            navigate("/sessions");
        } catch (e: unknown) {
            setError(e instanceof ApiError ? e.bodyText || e.message : "Erreur enregistrement");
        }
    });

    if (loading) {
        return (
            <Group justify="center" py="xl">
                <Loader />
            </Group>
        );
    }

    return (
        <Stack gap="md" maw={560}>
            <Button component={Link} to="/sessions" variant="subtle" size="xs" w="fit-content" px={0}>
                ← Sessions
            </Button>
            <Title order={2}>{isNew ? "Nouvelle session" : "Modifier la session"}</Title>
            {error ? (
                <Alert color="red" title="Erreur">
                    {error}
                </Alert>
            ) : null}
            <form onSubmit={submit}>
                <Stack gap="md">
                    <Select
                        label="Formation"
                        data={courses.map((c) => ({ value: String(c.courseId), label: c.name }))}
                        disabled={courses.length === 0}
                        {...form.getInputProps("courseId")}
                    />
                    <TextInput label="Début" type="datetime-local" required {...form.getInputProps("startDateLocal")} />
                    <Select
                        label="Mode"
                        data={[
                            { value: String(Del.InPerson), label: deliveryLabel(Del.InPerson) },
                            { value: String(Del.Remote), label: deliveryLabel(Del.Remote) },
                        ]}
                        {...form.getInputProps("deliveryMode")}
                    />
                    <Group justify="flex-end">
                        <Button component={Link} to="/sessions" variant="default">
                            Annuler
                        </Button>
                        <Button type="submit">Enregistrer</Button>
                    </Group>
                </Stack>
            </form>
        </Stack>
    );
}
