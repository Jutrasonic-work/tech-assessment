import { AppShell, Anchor, Group, Text, Title } from "@mantine/core";
import { Outlet } from "react-router-dom";
import { ThemeToggle } from "@/shared/components/ThemeToggle";

export function PublicLayout() {
    return (
        <AppShell header={{ height: 64 }} padding="md">
            <AppShell.Header px="md" py="xs">
                <Group h="100%" justify="space-between" align="center">
                    <Anchor href="/" underline="never" c="inherit">
                        <Title order={3}>WeChooz Formations</Title>
                    </Anchor>
                    <Group gap="sm">
                        <Anchor href="/admin" size="sm" c="dimmed">
                            Administration
                        </Anchor>
                        <ThemeToggle />
                    </Group>
                </Group>
            </AppShell.Header>
            <AppShell.Main>
                <Outlet />
            </AppShell.Main>
            <AppShell.Footer p="md" bg="var(--mantine-color-body)">
                <Text size="xs" c="dimmed" ta="center">
                    Dialogue social &amp; CSE — catalogue des sessions
                </Text>
            </AppShell.Footer>
        </AppShell>
    );
}
