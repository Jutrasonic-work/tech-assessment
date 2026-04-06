import { AppShell, Burger, Button, Group, NavLink as MantineNavLink, Stack, Text, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Link, Outlet } from "react-router-dom";
import { ThemeToggle } from "@/shared/components/ThemeToggle";
import { clearStoredRole, getStoredRole, type AdminRole } from "@/admin/auth/adminSession";

function NavSection({ role }: { role: AdminRole }) {
    const formation = role === "formation";
    return (
        <Stack gap={4} mt="md">
            {formation ? (
                <>
                    <MantineNavLink component={Link} to="/courses" label="Formations" />
                    <MantineNavLink component={Link} to="/sessions" label="Sessions" />
                </>
            ) : null}
            <MantineNavLink component={Link} to="/sales-access" label="Accès par n° de session" />
        </Stack>
    );
}

export function AdminChrome() {
    const [opened, { toggle }] = useDisclosure();
    const role = getStoredRole();

    const handleLeave = () => {
        clearStoredRole();
        window.location.assign("/");
    };

    return (
        <AppShell
            header={{ height: 56 }}
            navbar={{ width: 260, breakpoint: "sm", collapsed: { mobile: !opened } }}
            padding="md"
        >
            <AppShell.Header px="md" withBorder>
                <Group h="100%" justify="space-between">
                    <Group>
                        <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
                        <Title order={4}>WeChooz — Admin</Title>
                    </Group>
                    <Group gap="sm">
                        {role ? (
                            <Text size="sm" c="dimmed" visibleFrom="sm">
                                Connecté · {role}
                            </Text>
                        ) : null}
                        <ThemeToggle />
                        <Button variant="light" size="xs" onClick={handleLeave}>
                            Quitter (session locale)
                        </Button>
                    </Group>
                </Group>
            </AppShell.Header>
            <AppShell.Navbar p="md" withBorder>
                <Text size="xs" c="dimmed" tt="uppercase" fw={600}>
                    Navigation
                </Text>
                {role ? <NavSection role={role} /> : null}
            </AppShell.Navbar>
            <AppShell.Main>
                <Outlet />
            </AppShell.Main>
        </AppShell>
    );
}
