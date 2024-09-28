import { Formik, FormikProps } from "formik";
import React from "react";
import { SafeAreaView, TextInput, View, Text, ActivityIndicator, Button } from "react-native";
import * as yup from 'yup';

interface TrainingCreationModel {
  gymId: string,
  trainingStudentId: string,
  studentName: string,
  trainingData: undefined | TrainingCreationModel
}

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

const validationSchema = yup.object().shape({
  gymId: yup.string()
    .required("Selecione uma academia."),
  trainingStudentId: yup.string()
    .required("Insira um estudante.")
});

const GymTrainingCreationPage = () => {
  const initialValues: TrainingCreationModel = {
    gymId: "",
    trainingStudentId: "",
    studentName: "",
    trainingData: undefined
  };

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

  return (
    <SafeAreaView style={styles.container}>
      <Formik
        initialValues={initialValues}
        onSubmit={(values, actions) => {
          console.debug("handling user creation...")

          return handleSubmit(actions, values)
            .finally(() => actions.setSubmitting(false));
        }}
        validationSchema={validationSchema}
      >

        {formikProps => {
          return (
            <React.Fragment>
              <StyledInput
                formikKey={"firstName"}
                formikProps={formikProps}
                label={"Primeiro nome"}
                autoFocus />

              <StyledInput
                formikKey={"lastName"}
                formikProps={formikProps}
                label={"Sobrenome"} />

              <StyledInput
                formikKey={"email"}
                formikProps={formikProps}
                label={"E-mail"}
                keyboardType="email-address" />

              <StyledInput
                formikKey={"password"}
                formikProps={formikProps}
                label={"Senha"}
                secureTextEntry />

              <StyledInput
                formikKey={"confirmPassword"}
                formikProps={formikProps}
                label={"Confirme a senha"}
                secureTextEntry />

              <StyledInput
                formikKey={"phoneNumber"}
                formikProps={formikProps}
                label={"phoneNumber"}
                secureTextEntry />

              <View style={styles.buttonContainer}>
                {formikProps.isSubmitting ?
                  <ActivityIndicator /> :
                  <Button onPress={() => formikProps.handleSubmit()} title="Criar usuário" />}
              </View>

              <View>
                <Text style={{ color: 'red' }}>{formikProps.errors[responseErrorsKey as keyof UserCreateModel]}</Text>
              </View>
            </React.Fragment>
          );
        }}
      </Formik>
    </SafeAreaView>
  );
}

export default GymTrainingCreationPage;