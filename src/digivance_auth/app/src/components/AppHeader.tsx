import { AppShell, Burger, Button, Grid, Group } from '@mantine/core';
import * as React from 'react';

interface AppHeaderProps {
    isOpen: boolean;
    toggleOpen: () => void;
}

const AppHeader: React.FC<AppHeaderProps> = (props: AppHeaderProps) => {
    const { isOpen, toggleOpen } = props;
  return <AppShell.Header p="md">
       <Grid>
            <Grid.Col span="auto"><Burger opened={isOpen} onClick={toggleOpen} hiddenFrom="sm" size="sm" /></Grid.Col>
            <Grid.Col span="auto">
                <Group justify="flex-end" wrap="nowrap">
                    <Button variant="default">Account</Button>
                    <Button variant="default">Settings</Button>
                </Group>
            </Grid.Col>
        </Grid>
    </AppShell.Header>
};
export default AppHeader;