import { Modal, View, Text, Button, ActivityIndicator, TextInput } from "react-native";
import { styles } from "./styles";
import React from "react";
import { Formik, FormikProps } from "formik";

type ChildComponentProps = {
    modalVisible: boolean,
    setModalVisible: React.Dispatch<React.SetStateAction<boolean>>
};

interface GymCreateModel {
    gymName: string,
    description: string | undefined
}

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

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

const CreateGymModalWithManageAnyGymRole = ({ modalVisible, setModalVisible }: ChildComponentProps) => {
    return (
        <Modal
            animationType="slide" // You can use "slide", "fade", or "none"
            transparent={true}
            visible={modalVisible}
            onRequestClose={() => {
                setModalVisible(false);
            }}>

            <View style={styles.modalView}>

                <Formik
                    initialValues={{
                        description: undefined,
                        gymName: ""
                    } as GymCreateModel}
                    onSubmit={(values, actions) => {
                        console.debug("handling gym creation...")

                        return handleSubmit(actions, values)
                            .finally(() => actions.setSubmitting(false));
                    }
                    }
                    validationSchema={validationSchema}
                >

                    {formikProps => {
                        return (
                            <React.Fragment>
                                <StyledInput
                                    formikKey={"gymName"}
                                    formikProps={formikProps}
                                    label={"Nome da academia"}
                                    autoFocus />

                                <StyledInput
                                    formikKey={"description"}
                                    formikProps={formikProps}
                                    label={"Descrição"} />

                                <View style={styles.buttonContainer}>
                                    {formikProps.isSubmitting ?
                                        <ActivityIndicator /> :
                                        <Button onPress={() => formikProps.handleSubmit()} title="Criar acedemia" />}
                                </View>

                                <View>
                                    <Text style={{ color: 'red' }}>{formikProps.errors[responseErrorsKey as keyof GymCreateModel]}</Text>
                                </View>
                            </React.Fragment>
                        );
                    }}
                </Formik>

                <Button title="Fechar" onPress={() => setModalVisible(false)} />
            </View>
        </Modal>
    );
}

export default CreateGymModalWithManageAnyGymRole;