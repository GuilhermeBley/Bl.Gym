import { Modal, View, Text, Button, ActivityIndicator, TextInput } from "react-native";
import { styles } from "./styles";
import React from "react";
import { Formik, FormikHelpers, FormikProps } from "formik";
import * as yup from 'yup';

type ChildComponentProps = {
    modalVisible: boolean,
    setModalVisible: React.Dispatch<React.SetStateAction<boolean>>,
    onSubmiting: (model: GymCreateModel) => Promise<void>
};

export interface GymCreateModel {
    gymName: string,
    description: string | undefined
}

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

const validationSchema = yup.object().shape({
    gymName: yup.string()
      .min(2, "O nome deve conter 2 caracteres.")
      .max(45, "Insira um nome mais curto, por favor.")
      .required("Nome é obrigatório."),
    description: yup.string()
      .min(2, "Descrição muito curta...")
      .max(1000, "Insira uma descrição um pouco mais curta, por favor.")
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

const CreateGymModalWithManageAnyGymRole = ({
    modalVisible,
    setModalVisible,
    onSubmiting,
}: ChildComponentProps) => {
    
    const handleSubmit = (actions: FormikHelpers<GymCreateModel>, model: GymCreateModel) => {
        return onSubmiting(model);
    }

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