import React, { useEffect, useState } from "react";
import { View, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";
import { handleLogin, handleRefreshToken, LoginResultStatus } from "./action";
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { SafeAreaView } from "react-native-safe-area-context";
import { CREATE_USER_SCREEN, HOME_SCREEN } from "../../routes/RoutesConstant";
import axios from "axios"
import {
  TextInput as PaperTextInput,
  Button as PaperButton,
  Text,
  Divider,
  HelperText,
  useTheme
} from 'react-native-paper';

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
const theme = useTheme();
  const [buttonErrorMessage, setButtonErrorMessage] = useState("");
  const { login, user } = useContext(UserContext);
  const [pageData, setPageData] = useState({
    isRunningFirstLoading: false
  } as PageData);
  const [secureTextEntry, setSecureTextEntry] = useState(true);

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
    const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
    const errorMessage = typeof error === 'string' ? error : '';

    return (
      <View style={styles.inputContainer}>
        <PaperTextInput
          mode="outlined"
          label={label}
          value={formikProps.values[formikKey]}
          onChangeText={formikProps.handleChange(formikKey)}
          onBlur={formikProps.handleBlur(formikKey)}
          error={!!error}
          style={styles.input}
          {...rest}
          right={
            formikKey === 'password' ? 
            <PaperTextInput.Icon 
              icon={secureTextEntry ? "eye-off" : "eye"} 
              onPress={() => setSecureTextEntry(!secureTextEntry)}
            /> : undefined
          }
        />
        {error && <HelperText type="error" visible={!!error}>
          {errorMessage}
        </HelperText>}
      </View>
    );
  }

  useEffect(() => {
    let cts = axios.CancelToken.source();

    let timeout = setTimeout(() => {
      cts.cancel("Request timed out");
    }, 1000 * 60);

    let fetchInitialData = async () => {
      setPageData(previous => ({
        ...previous,
        isRunningFirstLoading: true
      }));

      console.debug(`Trying to refresh token (${user.refreshToken})`)

      if (user.refreshToken !== undefined &&
        user.dueDate !== undefined &&
        user.isExpirated()
      ) {
        let response = await handleRefreshToken(user.refreshToken, user.id, cts.token);

        if (response.Status !== LoginResultStatus.Success) {
          console.debug("Failed to refresh token.");
          return;
        }
    
        console.debug(`Trying to login with token ${response.Token.length}`)
        await login(response.Token, response.RefreshToken)
      }
      else{
        console.debug(`Token was not refreshed ${user.isExpirated()} - ${user.dueDate}`)
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

  if (pageData.isRunningFirstLoading) {
    return (
      <SafeAreaView style={styles.container}>
        <View>
          <ActivityIndicator animating={true} />
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
              secureTextEntry={secureTextEntry}
            />

            <View style={styles.buttonContainer}>
              {formikProps.isSubmitting ?
                <ActivityIndicator animating={true} /> :
                <PaperButton 
                  mode="contained" 
                  onPress={() => formikProps.handleSubmit()}
                >
                  Entrar
                </PaperButton>}

              <View style={styles.separatorContainer}>
                <Divider style={styles.line} />
                <Text style={styles.separatorText}>ou</Text>
                <Divider style={styles.line} />
              </View>

              <PaperButton 
                mode="outlined" 
                onPress={() => navigation.navigate(CREATE_USER_SCREEN)}
              >
                Criar uma conta
              </PaperButton>
              
              {buttonErrorMessage && (
                <Text style={{ color: theme.colors.error, marginTop: 10 }}>
                  {buttonErrorMessage}
                </Text>
              )}
            </View>
          </React.Fragment>
        )}
      </Formik>
    </SafeAreaView>
  );
};

export default LoginScreen;