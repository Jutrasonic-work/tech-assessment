import { ActionIcon, useComputedColorScheme, useMantineColorScheme } from "@mantine/core";
import { IconMoon, IconSun } from "@tabler/icons-react";

export function ThemeToggle() {
    const { setColorScheme } = useMantineColorScheme();
    const computed = useComputedColorScheme("light", { getInitialValueInEffect: true });

    return (
        <ActionIcon
            onClick={() => setColorScheme(computed === "light" ? "dark" : "light")}
            variant="default"
            size="lg"
            radius="md"
            aria-label="Basculer le thème"
        >
            {computed === "light" ? <IconMoon size={18} /> : <IconSun size={18} />}
        </ActionIcon>
    );
}
