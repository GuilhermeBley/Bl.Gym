import { Picker } from "@react-native-picker/picker";
import { View, Text } from "react-native";
import { styles } from "./styles";

interface StyledSelectProps {
    formikKey: string;
    formikProps: any;
    label: string;
    options: { label: string; value: string }[];
    [key: string]: any;
}

const StyledSelectInputFormik: React.FC<StyledSelectProps> = (
    { formikKey, formikProps, label, options, ...rest }
) => {
    const inputStyles = styles.input;

    const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
    const errorMessage = typeof error === 'string' ? error : '';

    if (errorMessage) {
        inputStyles.borderColor = 'red';
    }

    return (
        <View style={styles.inputContainer}>
            <Text>{label}</Text>
            <View style={[inputStyles, { paddingHorizontal: 5 }]}>
                <Picker
                    selectedValue={formikProps.values[formikKey]}
                    onValueChange={formikProps.handleChange(formikKey)}
                    onBlur={formikProps.handleBlur(formikKey)}
                    {...rest}
                >
                    {options.map((option) => (
                        <Picker.Item key={option.value} label={option.label} value={option.value} />
                    ))}
                </Picker>
            </View>
            {errorMessage ? <Text style={{ color: 'red' }}>{errorMessage}</Text> : null}
        </View>
    );
}

export default StyledSelectInputFormik;