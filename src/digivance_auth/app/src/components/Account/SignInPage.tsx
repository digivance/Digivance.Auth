import { Button, Container, Divider, Fieldset } from '@mantine/core';
import * as React from 'react';
import { FaEnvelope, FaKey } from 'react-icons/fa';
import TextField from '../common/TextField';
const SignInPage: React.FC = () => {

  return <>
        <Container>
            <form action="">
                <Fieldset legend="Sign In" >
                    <TextField
                        icon={<FaEnvelope />}
                        label="Email Address"
                        description="Please enter a valid email address"
                        placeholder="you@domain.com"
                        name='emailAddress'
                        type='email'
                        mb={"md"}
                    />

                    <TextField
                        icon={<FaKey />}
                        label="Password"
                        description="Please create a strong password"
                        name="password"
                        type="password"
                        mb="md"
                    />
                    <Divider mb="lg" />
                    <Button>Sign In</Button>
                    
                </Fieldset>
            </form>
        </Container>
    </>
};
export default SignInPage;