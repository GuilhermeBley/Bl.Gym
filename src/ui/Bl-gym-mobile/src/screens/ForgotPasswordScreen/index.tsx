import React, { useState } from "react";
import { View, Text, TextInput, Button, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";
import { Formik, FormikProps } from 'formik';
import * as yup from 'yup';
import { SafeAreaView } from "react-native-safe-area-context";
import { LOGIN_SCREEN } from "../../routes/RoutesConstant";
import { handleRequestToChangePassword } from "./action";

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
});

const ForgotPasswordScreen = ({ navigation }: any) => {

  const [buttonErrorMessage, setButtonErrorMessage] = useState("");

  const userContext = useContext(UserContext);

  const handleChangePasswordAndNavigate = async (
    login: string
  ) => {
    return handleRequestToChangePassword({ email: login })
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

  console.debug("ForgotPasswordScreen");
  return (
    <SafeAreaView style={styles.container}>
      <Formik
        initialValues={{
          login: ""
        }}
        onSubmit={(values, actions) =>
          handleChangePasswordAndNavigate(values.login)
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
                  title="Efetuar login"
                  onPress={()=>navigation.navigate(LOGIN_SCREEN)}/>
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

export default ForgotPasswordScreen;