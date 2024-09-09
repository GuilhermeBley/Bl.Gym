import React, { useEffect, useState } from "react";
import { View, Text, TextInput, Button, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";
import { handleLogin, handleRefreshToken, LoginResultStatus } from "./action";
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { SafeAreaView } from "react-native-safe-area-context";
import { CREATE_USER_SCREEN, HOME_SCREEN } from "../../routes/RoutesConstant";

interface StyledInputProps {
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

interface PageData {
  isRunningFirstLoading: boolean
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
  const { login, user, logout } = useContext(UserContext);
  const [pageData, setPageData] = useState({
    isRunningFirstLoading: false
  } as PageData)

  const handleLoginAndNavigate = async (
    loginInput: string,
    passwordInput: string
  ) => {

    let response = await handleLogin(loginInput, passwordInput);

    if (response.Status === LoginResultStatus.InvalidLoginOrPassword) {
      setButtonErrorMessage("Usuário ou senha inválidos.");
      return;
    }
    if (response.Status !== LoginResultStatus.Success) {
      setButtonErrorMessage("Falha ao realizar o login.");
      return;
    }

    await login(response.Token, response.RefreshToken);
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

  useEffect(() => {

    let fetchInitialData = async () => {
      setPageData(previous => ({
        ...previous,
        isRunningFirstLoading: true
      }));

      if (user.refreshToken !== undefined &&
        user.dueDate !== undefined &&
        user.isExpirated()
      ) {
        let response = await handleRefreshToken(user.refreshToken, user.id);

        if (response.Status !== LoginResultStatus.Success) {
          return;
        }
    
        await login(response.Token, response.RefreshToken)
      }
    }

    fetchInitialData()
      .finally(() => {
        setPageData(previous => ({
          ...previous,
          isRunningFirstLoading: false
        }));
      })
  }, [])

  console.debug("LoginScreen");

  if (pageData.isRunningFirstLoading)
  {
    return (
      <SafeAreaView style={styles.container}>
        <View>
          <ActivityIndicator></ActivityIndicator>
        </View>
      </SafeAreaView>
    );
  }

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
                onPress={() => navigation.navigate(CREATE_USER_SCREEN)} />
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