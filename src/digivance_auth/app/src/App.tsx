import { AppShell, AppShellProps, Burger, Center, Container, Flex, MantineProvider } from "@mantine/core";
import React from "react";
import { useDisclosure } from "@mantine/hooks";
import '@mantine/core/styles.css';
import AppNavBar from "./components/AppNavBar";
import AppHeader from "./components/AppHeader";
import AppFooter from "./components/AppFooter";
import AppMain from "./components/AppMain";

const App: React.FC = () => {
  const [opened, { toggle }] = useDisclosure();

    
    return <MantineProvider defaultColorScheme="dark">
    <AppShell
      header={{ height: 60 }}
      navbar={{
        width: 300,
        breakpoint: 'sm',
        collapsed: { mobile: !opened },
      }}
      padding="md"
    >  
      <AppHeader />
      <AppNavBar />
      <AppMain/>
      <AppFooter/>
    </AppShell>
    </MantineProvider>
};

export default App
