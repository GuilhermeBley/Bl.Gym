import React, { useState } from "react";
import { Alert, View, Text, TextInput, Button, StyleSheet, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";
import { handleLogin, LoginResultStatus } from "./action";
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { SafeAreaView } from "react-native-safe-area-context";

const LoginScreen = () => {

  interface StyledInputProps {
    formikKey: string,
    formikProps: FormikProps<any>;
    label: string;
    [key: string]: any;
  }

  const [userLoginInput, setUserLoginInput] = useState("");
  const [userPasswordInput, setUserPasswordInput] = useState("");

  const userContext = useContext(UserContext);

  const handleLoginAndNavigate = async () => {
    let response = await handleLogin(userLoginInput, userPasswordInput);
    
    if (response.Status === LoginResultStatus.InvalidLoginOrPassword) {
      Alert.alert("Falha no Login", "Usuário ou senha inválidos.");
      return;
    }
    if (response.Status !== LoginResultStatus.Success) {
      Alert.alert("Falha no Login", "Falha ao realizar o login.");
      return;
    }
  }

  const StyledInput: React.FC<StyledInputProps> = ({ formikKey, formikProps, label, ...rest }) => {
    const inputStyles = styles.input;
    
    const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
    const errorMessage = typeof error === 'string' ? error : '';
    
    if (error !== '')
    {
      inputStyles.borderColor = "red"
    }

    return (
      <View>
        <Text>{label}</Text>
        <TextInput
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
    <SafeAreaView style={{marginTop: 90}}>
      <Formik
        initialValues={{
          login: "",
          password: ""
        }}
        onSubmit={(values, actions) => 
          handleLogin(values.login, values.password)
          .finally(() => actions.setSubmitting(false))}
      >

        {formikProps => (
          <React.Fragment>
            <StyledInput
              formikKey={"login"}
              formikProps={formikProps}
              label={"Login"}
              autoFocus
            />
            
            <StyledInput
              formikKey={"password"}
              formikProps={formikProps}
              label={"Senha"}
              secureTextEntry
            />

            {formikProps.isSubmitting ? 
              <ActivityIndicator /> :
              <Button onPress={() => formikProps.handleSubmit()} title="Entrar" />}
          </React.Fragment>
        )}
      </Formik>
    </SafeAreaView>
  );
};

export default LoginScreen;