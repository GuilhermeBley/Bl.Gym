import React, { useState } from 'react';
import { View, Text, TextInput, Button, SafeAreaView, ActivityIndicator } from 'react-native';
import styles from '../LoginScreen/styles';
import { handleCreateUser } from './action';
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { LOGIN_SCREEN } from '../../routes/RoutesConstant';

interface UserCreateModel {
  firstName: string,
  lastName: string,
  email: string,
  password: string,
  confirmPassword: string,
  phoneNumber: string | number | null,
}

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

const validationSchema = yup.object().shape({
  firstName: yup.string()
    .min(2, "O nome deve conter 2 caracteres.")
    .max(45, "Insira um nome mais curto, por favor.")
    .required("Nome é obrigatório."),
  lastName: yup.string()
    .min(2, "O sobrenome deve conter 2 caracteres.")
    .max(45, "Insira um sobrenome mais curto, por favor.")
    .required("Sobrenome é obrigatório."),
  email: yup.string().email("E-mail inválido.").required("E-mail é obrigatório."),
  password: yup.string()
    .required('Password is required')
    .min(8, 'A senha deve conter no mínimo 8 caracteres.')
    .max(45, 'A senha está muito longa.')
    .matches(/[a-zA-Z]/, 'A senha deve conter uma letra.')
    .matches(/[0-9]/, 'A senha deve conter um número.')
    .matches(/[!@#$%^&*(),.?":{}|<>]/, 'Senha deve conter pelo menos um caracter especial.'),
  confirmPassword: yup.string().test("passwords-match", "As senhas devem ser iguais.",
    function (value) {
      return this.parent.password === value;
    }).required("Confirmação de senha obrigatório."),
  phoneNumber: yup.number().notRequired(),
});

const CreateUserScreen = ({ navigator }: any) => {

  const initialValues: UserCreateModel = {
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: null,
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

  const handleSubmit = async (data: UserCreateModel) => {
    var result = await handleCreateUser(data.firstName, data.lastName, data.email, data.password, data.phoneNumber?.toString() ?? null)
    console.debug(result);

    if (result.Success) {
      navigator.navigate(LOGIN_SCREEN);
    }
  };

  return (
    <SafeAreaView style={styles.container}>
      <Formik
        initialValues={initialValues}
        onSubmit={(values, actions) => {
            console.debug("handling user creation...")  

            return handleSubmit(values)
              .finally(() => actions.setSubmitting(false))
          }
        }
        validationSchema={validationSchema}
      >

        {formikProps => (
          <React.Fragment>
            <StyledInput
              formikKey={"firstName"}
              formikProps={formikProps}
              label={"Primeiro nome"}
              autoFocus
            />

            <StyledInput
              formikKey={"lastName"}
              formikProps={formikProps}
              label={"Sobrenome"}
            />

            <StyledInput
              formikKey={"email"}
              formikProps={formikProps}
              label={"E-mail"}
              keyboardType="email-address"
            />

            <StyledInput
              formikKey={"password"}
              formikProps={formikProps}
              label={"Senha"}
              secureTextEntry
            />

            <StyledInput
              formikKey={"confirmPassword"}
              formikProps={formikProps}
              label={"Confirme a senha"}
              secureTextEntry
            />

            <StyledInput
              formikKey={"phoneNumber"}
              formikProps={formikProps}
              label={"phoneNumber"}
              secureTextEntry
            />

            <View style={styles.buttonContainer}>
              {formikProps.isSubmitting ?
                <ActivityIndicator /> :
                <Button onPress={() => formikProps.handleSubmit()} title="Criar usuário" />}
            </View>
          </React.Fragment>
        )}
      </Formik>
    </SafeAreaView>
  );
};

export default CreateUserScreen;