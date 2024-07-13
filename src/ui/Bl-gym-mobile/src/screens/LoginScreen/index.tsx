import React, { useState } from "react";
import { View, Text, TextInput, Button, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";
import { handleLogin, LoginResultStatus } from "./action";
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { SafeAreaView } from "react-native-safe-area-context";
import { CREATE_USER_SCREEN } from "../../routes/AuthStack";
import { HOME_SCREEN } from "../../routes/SelectGymTabs";

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

const validationSchema = yup.object().shape({
  login: yup.string()
    .email("E-mail inválido.")
    .required("Login é obrigatório."),
  password: yup.string()
    .min(8, "Senha inválida, deve conter no mínimo 8 caracteres.")
    .required("Senha é obrigatório."),
});

const LoginScreen = ({ navigation }: any) => {

  const [buttonErrorMessage, setButtonErrorMessage] = useState("");

  const userContext = useContext(UserContext);

  const handleLoginAndNavigate = async (
    login: string,
    password: string
  ) => {
    let response = await handleLogin(login, password);

    if (response.Status === LoginResultStatus.InvalidLoginOrPassword) {
      setButtonErrorMessage("Usuário ou senha inválidos.");
      return;
    }
    if (response.Status !== LoginResultStatus.Success) {
      setButtonErrorMessage("Falha ao realizar o login.");
      return;
    }

    userContext.setUserByJwtToken(response.Token);
    navigation.navigate(HOME_SCREEN)
  }

  const StyledInput: React.FC<StyledInputProps> = ({ formikKey, formikProps, label, ...rest }) => {
    const inputStyles = styles.input;

    const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
    const errorMessage = typeof error === 'string' ? error : '';

    if (errorMessage.length > 0) {
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
        <Text style={{ color: 'red' }}>{errorMessage}</Text>
      </View>
    );
  }

  console.debug("LoginScreen");
  return (
    <SafeAreaView style={styles.container}>
      <Formik
        initialValues={{
          login: "",
          password: ""
        }}
        onSubmit={(values, actions) =>
          handleLoginAndNavigate(values.login, values.password)
            .finally(() => actions.setSubmitting(false))}
        validationSchema={validationSchema}
      >

        {formikProps => (
          <React.Fragment>
            <StyledInput
              formikKey={"login"}
              formikProps={formikProps}
              label={"Login"}
              keyboardType="email-address"
              autoFocus
            />

            <StyledInput
              formikKey={"password"}
              formikProps={formikProps}
              label={"Senha"}
              secureTextEntry
            />

            <View style={styles.buttonContainer}>
              {formikProps.isSubmitting ?
                <ActivityIndicator /> :
                <Button onPress={() => formikProps.handleSubmit()} title="Entrar" />}

                <View style={styles.separatorContainer}>
                  <View style={styles.line} />
                  <Text style={styles.separatorText}>ou</Text>
                  <View style={styles.line} />
                </View>

                <Button
                  title="Criar uma conta"
                  onPress={()=>navigation.navigate(CREATE_USER_SCREEN)}/>
              <Text style={{ color: "red", width: "auto" }}>
                {buttonErrorMessage}
              </Text>
            </View>
          </React.Fragment>
        )}
      </Formik>
    </SafeAreaView>
  );
};

export default LoginScreen;