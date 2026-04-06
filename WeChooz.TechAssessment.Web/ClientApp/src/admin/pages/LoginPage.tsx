import { Alert, Button, Card, Group, PasswordInput, Stack, Text, Title } from "@mantine/core";
import { useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { login } from "@/shared/api/authApi";
import { ApiError } from "@/shared/api/http";
import { parseLoginRole, getStoredRole, setStoredRole } from "@/admin/auth/adminSession";
import { ThemeToggle } from "@/shared/components/ThemeToggle";

export function LoginPage() {
    const navigate = useNavigate();
    const [value, setValue] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    if (getStoredRole()) {
        return <Navigate to="/" replace />;
    }

    const submit = async () => {
        setError(null);
        setLoading(true);
        try {
            const res = await login(value.trim());
            const role = parseLoginRole(res.login);
            if (!role) {
                setError("Réponse de connexion inattendue.");
                return;
            }
            setStoredRole(role);
            navigate("/", { replace: true });
        } catch (e) {
            if (e instanceof ApiError) {
                if (e.status === 401) {
                    setError("Identifiant inconnu. Utilisez « formation » ou « sales ».");
                } else {
                    setError(e.message);
                }
            } else {
                setError("Erreur réseau");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <Stack align="center" justify="center" mih="70vh" gap="lg" pos="relative">
            <Group pos="absolute" top={0} right={0}>
                <ThemeToggle />
            </Group>
            <Card withBorder shadow="sm" padding="xl" radius="md" maw={420} w="100%">
                <Stack gap="md">
                    <div>
                        <Title order={2}>Connexion admin</Title>
                        <Text size="sm" c="dimmed" mt="xs">
                            Saisis ton identifiant de test (sans mot de passe) : <strong>formation</strong> ou <strong>sales</strong>.
                        </Text>
                    </div>
                    {error ? (
                        <Alert color="red" title="Connexion refusée">
                            {error}
                        </Alert>
                    ) : null}
                    <PasswordInput
                        label="Identifiant"
                        placeholder="formation ou sales"
                        visible={true}
                        value={value}
                        onChange={(e) => setValue(e.currentTarget.value)}
                        onKeyDown={(e) => e.key === "Enter" && submit()}
                    />
                    <Button loading={loading} onClick={submit} fullWidth>
                        Se connecter
                    </Button>
                </Stack>
            </Card>
        </Stack>
    );
}
