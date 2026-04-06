import { Alert, Button, Group, NumberInput, Stack, Text, Title } from "@mantine/core";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export function SalesSessionAccessPage() {
    const navigate = useNavigate();
    const [sessionId, setSessionId] = useState<number | "">("");

    const go = () => {
        if (sessionId === "" || !Number.isFinite(Number(sessionId))) {
            return;
        }
        navigate(`/sessions/${sessionId}/participants`);
    };

    return (
        <Stack gap="md" maw={480}>
            <div>
                <Title order={2}>Accès session</Title>
                <Text size="sm" c="dimmed" mt="xs">
                    Avec le rôle <strong>sales</strong>, saisissez l'identifiant numérique de la session pour gérer les participants.
                </Text>
            </div>
            <Alert color="blue" variant="light">
                Vous pouvez récupérer l'ID depuis l'URL du détail session côté formation, ou depuis la base.
            </Alert>
            <NumberInput
                label="ID de session"
                placeholder="ex. 12"
                min={1}
                value={sessionId}
                onChange={(value) => setSessionId(typeof value === "number" ? value : "")}
            />
            <Group>
                <Button onClick={go}>Ouvrir les participants</Button>
            </Group>
        </Stack>
    );
}
