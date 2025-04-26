import { Button, Collapse, Container, Divider, Fieldset, Flex, Grid, Text } from '@mantine/core';
import * as React from 'react';
import { FaAt, FaEnvelope, FaKey, FaMobile } from 'react-icons/fa';
import { FaPerson } from 'react-icons/fa6';
import TextField from '../common/TextField';

const SignUpPage: React.FC = () => {
    const [showAdvancedForm, setShowAdvancedForm ] = React.useState(false);
    const toggleAdvanced = React.useCallback(() => setShowAdvancedForm(!showAdvancedForm), [showAdvancedForm]);

    return <Container mt="lg" mb={"xl"}>
        <form>
            <Fieldset legend="Create Account">
                <TextField
                    icon={<FaEnvelope />}
                    label="Email Address"
                    description="Please enter a valid email address"
                    placeholder="you@domain.com"
                    name="emailAddress"
                    type="email"
                    mb="md"
                />

                <TextField
                    icon={<FaKey />}
                    label="Password"
                    description="Please create a strong password"
                    name="password"
                    type="password"
                    mb="md"
                />

                <TextField
                    icon={<FaKey />}
                    label="Confirm Password"
                    description="Please confirm your password here"
                    name="confirmPassword"
                    type="password"
                    mb="md"
                />

                <Divider mb="lg" />
                    
                <Collapse in={showAdvancedForm}>
                    <TextField
                        icon={<FaAt />}
                        label="Username"
                        description="(Optional) provide a unique username. Will be automatically generated if left blank"
                        placeholder="@username"
                        name="username"
                        mb="md"
                    />

                    <TextField
                        icon={<FaMobile />}
                        label="Mobile Phone"
                        description="(Optional) please provide your mobile phone number"
                        name="phoneNumber"
                        mb="md"
                    />

                    <Grid>
                        <Grid.Col span={6}>
                            <TextField
                                icon={<FaPerson />}
                                label="First name"
                                description="(Optional) your first, or given name"
                                name="firstName"
                                mb="md"
                            />
                        </Grid.Col>

                        <Grid.Col span={6}>
                            <TextField
                                icon={<FaPerson />}
                                label="Last name"
                                description="(Optional) your last, or family name"
                                name="lastName"
                                mb="md"
                            />
                        </Grid.Col>
                    </Grid>
                </Collapse>

                <Flex justify="flex-end">
                    <Text c="red">Errors will be shown here</Text>
                </Flex>
                <Flex justify="flex-end">
                    <Button color="violet" me="md" onClick={toggleAdvanced}>
                        {showAdvancedForm ? 'Simple Form' : 'Advanced Form'}
                    </Button>

                    <Button>Sign Up</Button>
                </Flex>
            </Fieldset>
        </form>
    </Container>;
};

export default SignUpPage;
