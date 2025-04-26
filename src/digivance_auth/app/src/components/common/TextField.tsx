import * as React from 'react';
import { TextInput, TextInputProps } from "@mantine/core";

export interface TextFieldProps extends TextInputProps {
    icon?: React.ReactNode;
}

const TextField: React.FC<TextFieldProps> = (props: TextFieldProps) => {
    const label = props.icon ?
        <span>{props.icon} {props.label}</span> :
        props.label;

    return <TextInput
        {...props}
        label={label}
    />;
};

export default TextField;
