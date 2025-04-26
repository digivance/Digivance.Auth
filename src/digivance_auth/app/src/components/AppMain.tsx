import { AppShell } from '@mantine/core';
import * as React from 'react';
import { Route, Routes } from 'react-router-dom';
import SignInPage from './Account/SignInPage';
import SignUpPage from './Account/SignUpPage';
const AppMain: React.FC = () => {
  return       <AppShell.Main>
  <Routes>
      <Route path='/signin' element={<SignInPage/>}></Route>
      <Route path='/signup' element={<SignUpPage/>}></Route>
      </Routes>
</AppShell.Main>
};
export default AppMain;