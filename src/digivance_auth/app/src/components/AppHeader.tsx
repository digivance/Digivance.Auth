import { AppShell, Button, Container, Group } from '@mantine/core';
import * as React from 'react';
const AppHeader: React.FC = () => {
  return <AppShell.Header p="md">
        <Container className=''>
            <Group justify="flex-end">
            <Button variant="default">Account</Button>
            <Button variant="default">Settings</Button>
            </Group>
        </Container>
    </AppShell.Header>
};
export default AppHeader;