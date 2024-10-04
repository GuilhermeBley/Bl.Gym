import { Formik, FormikHelpers, FormikProps } from "formik";
import React, { useState } from "react";
import { SafeAreaView, TextInput, View, Text, ActivityIndicator, Button } from "react-native";
import * as yup from 'yup';
import { styles } from "./styles";
import { Picker } from '@react-native-picker/picker';
import { GetCurrentUserGymResponse } from "../GymScreen/action";
import { getGymMembers, GetGymMembersResponse } from "./actions";

interface FormDataModel {
  availableGyms: GetCurrentUserGymResponse[],
  availableUsers: GetGymMembersResponse[],
  selectedGym: string | undefined,
  selectedStudent: string | undefined
}

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

interface StyledSelectProps {
  formikKey: string;
  formikProps: any;
  label: string;
  options: { label: string; value: string }[];
  [key: string]: any;
}

const validationSchema = yup.object().shape({
  gymId: yup.string()
    .required("Selecione uma academia."),
  trainingStudentId: yup.string()
    .required("Insira um estudante.")
});

const GymTrainingCreationPage = () => {
  
  const responseErrorsKey = "api-errors"
  
  const initialValues: TrainingCreationModel = {
    gymId: "",
    trainingStudentId: "",
    studentName: "",
    trainingData: undefined
  };

  const [formData, setFormData] = useState(
    {
      availableGyms: [],
      availableUsers: [],
      selectedGym: undefined,
      selectedStudent: undefined
    } as FormDataModel
  );
  
  const StyledSelect: React.FC<StyledSelectProps> = ({ formikKey, formikProps, label, options, ...rest }) => {
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

  const resetFormData = () => {
    setFormData(previous => ({
      ...previous,
      selectedGym: undefined,
      selectedStudent: undefined,
      availableUsers: []
    }));
  }

  const handleGymSelect = async (
    selectedGymId: string
  ) => {
    if (!selectedGymId)
    {
      resetFormData();
      return;
    }

    var membersResult = await getGymMembers(selectedGymId);

    if (membersResult.ContainsError)
    {
      resetFormData();
      return;
    }

    setFormData(previous => ({
      ...previous,
      selectedGym: selectedGymId,
      availableUsers: membersResult.Data
    }));
  }

  const handleSubmit = async (
    formikHelper: FormikHelpers<any>,
    data: TrainingCreationModel
  ) => {

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
              <StyledSelect
                formikKey={"gymId"}
                formikProps={formikProps}
                label={"Academia"}
                options={formData.availableGyms.map(e =>
                  ({ label: e.Name, value: e.Id })
                )}
                autoFocus
                onValueChange={handleGymSelect} />
              
              <StyledSelect
                formikKey={"studentId"}
                formikProps={formikProps}
                label={"Selecione o estudante"}
                options={formData.availableGyms.map(e =>
                  ({ label: e.Name, value: e.Id })
                )}
                editable={false} />

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
                <Text style={{ color: 'red' }}>{formikProps.errors[responseErrorsKey as keyof TrainingCreationModel]}</Text>
              </View>
            </React.Fragment>
          );
        }}
      </Formik>
    </SafeAreaView>
  );
}

export default GymTrainingCreationPage;