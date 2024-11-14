import { FormikProps } from "formik";
import { View, Text, TextInput, StyleSheet } from "react-native";


interface StyledInputProps {
    formikKey: string,
    formikProps: FormikProps<any>;
    label: string;
    [key: string]: any;
  }

const styles = StyleSheet.create({
    input: {
        height: 40,
        borderColor: 'gray',
        borderWidth: 1,
        paddingHorizontal: 8,
    },
    inputContainer: {
      marginBottom: 12,
    },
});

const StyledInput: React.FC<StyledInputProps> = ({ formikKey, formikProps, label, ...rest }) => {
    const inputStyles = styles.input;

    const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
    const errorMessage = typeof error === 'string' ? error : '';

    if (errorMessage) {
        inputStyles.borderColor = "red"
    }

    return (
        <View style={styles.inputContainer}>
            <Text>{label}</Text>
            <TextInput
                style={inputStyles}
                value={formikProps.values[formikKey]}
                onChangeText={formikProps.handleChange(formikKey)}
                onBlur={formikProps.handleBlur(formikKey)}
                {...rest}
            >

            </TextInput>
            {errorMessage ? <Text style={{ color: 'red' }}>{errorMessage}</Text> : null}
        </View>
    );
}

export default StyledInput;