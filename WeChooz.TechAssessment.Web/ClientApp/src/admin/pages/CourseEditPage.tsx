import { Alert, Button, Group, Loader, NumberInput, Select, Stack, Textarea, TextInput, Title } from "@mantine/core";
import { useForm } from "@mantine/form";
import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { createCourse, getCourseById, updateCourse } from "@/shared/api/adminCoursesApi";
import { ApiError } from "@/shared/api/http";
import type { CseAudience } from "@/shared/api/types";
import { CseAudience as Aud } from "@/shared/api/types";
import { audienceLabel } from "@/shared/labels";

type FormValues = {
    name: string;
    shortDescription: string;
    longDescriptionMarkdown: string;
    durationDays: number;
    cseAudience: string;
    maxCapacity: number;
    trainerFirstName: string;
    trainerLastName: string;
};

export function CourseEditPage() {
    const { courseId } = useParams<{ courseId: string }>();
    const isNew = courseId === undefined;
    const navigate = useNavigate();
    const [loading, setLoading] = useState(!isNew);
    const [error, setError] = useState<string | null>(null);

    const form = useForm<FormValues>({
        initialValues: {
            name: "",
            shortDescription: "",
            longDescriptionMarkdown: "",
            durationDays: 1,
            cseAudience: String(Aud.DelegateElu),
            maxCapacity: 10,
            trainerFirstName: "",
            trainerLastName: "",
        },
        validate: {
            name: (v) => (!v?.trim() ? "Requis" : null),
            shortDescription: (v) => (!v?.trim() ? "Requis" : null),
            trainerFirstName: (v) => (!v?.trim() ? "Requis" : null),
            trainerLastName: (v) => (!v?.trim() ? "Requis" : null),
        },
    });

    useEffect(() => {
        if (isNew || !courseId) {
            return;
        }
        const id = Number(courseId);
        setLoading(true);
        getCourseById(id)
            .then((c) => {
                form.setValues({
                    name: c.name,
                    shortDescription: c.shortDescription,
                    longDescriptionMarkdown: c.longDescriptionMarkdown,
                    durationDays: c.durationDays,
                    cseAudience: String(c.cseAudience),
                    maxCapacity: c.maxCapacity,
                    trainerFirstName: c.trainerFirstName,
                    trainerLastName: c.trainerLastName,
                });
            })
            .catch((e: unknown) => {
                setError(e instanceof ApiError ? e.message : "Erreur chargement");
            })
            .finally(() => setLoading(false));
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [courseId, isNew]);

    const submit = form.onSubmit(async (values) => {
        setError(null);
        const body = {
            name: values.name.trim(),
            shortDescription: values.shortDescription.trim(),
            longDescriptionMarkdown: values.longDescriptionMarkdown,
            durationDays: values.durationDays,
            cseAudience: Number(values.cseAudience) as CseAudience,
            maxCapacity: values.maxCapacity,
            trainerFirstName: values.trainerFirstName.trim(),
            trainerLastName: values.trainerLastName.trim(),
        };
        try {
            if (isNew) {
                await createCourse(body);
            } else {
                await updateCourse(Number(courseId), body);
            }
            navigate("/courses");
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
        <Stack gap="md" maw={720}>
            <Button component={Link} to="/courses" variant="subtle" size="xs" w="fit-content" px={0}>
                ← Liste des formations
            </Button>
            <Title order={2}>{isNew ? "Nouvelle formation" : "Modifier la formation"}</Title>
            {error ? (
                <Alert color="red" title="Erreur">
                    {error}
                </Alert>
            ) : null}
            <form onSubmit={submit}>
                <Stack gap="md">
                    <TextInput label="Nom" required {...form.getInputProps("name")} />
                    <Textarea label="Chapo (description courte)" required minRows={2} {...form.getInputProps("shortDescription")} />
                    <Textarea label="Description longue (Markdown)" minRows={8} {...form.getInputProps("longDescriptionMarkdown")} />
                    <Group grow>
                        <NumberInput label="Durée (jours)" min={1} required {...form.getInputProps("durationDays")} />
                        <NumberInput label="Capacité max" min={1} required {...form.getInputProps("maxCapacity")} />
                    </Group>
                    <Select
                        label="Population cible"
                        data={[
                            { value: String(Aud.DelegateElu), label: audienceLabel(Aud.DelegateElu) },
                            { value: String(Aud.President), label: audienceLabel(Aud.President) },
                        ]}
                        {...form.getInputProps("cseAudience")}
                    />
                    <Group grow>
                        <TextInput label="Prénom formateur" required {...form.getInputProps("trainerFirstName")} />
                        <TextInput label="Nom formateur" required {...form.getInputProps("trainerLastName")} />
                    </Group>
                    <Group justify="flex-end">
                        <Button component={Link} to="/courses" variant="default">
                            Annuler
                        </Button>
                        <Button type="submit">Enregistrer</Button>
                    </Group>
                </Stack>
            </form>
        </Stack>
    );
}
