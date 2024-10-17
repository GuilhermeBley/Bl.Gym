import { FieldArray, Formik, FormikHelpers, FormikProps } from "formik";
import React, { useState } from "react";
import { SafeAreaView, TextInput, View, Text, ActivityIndicator, Button, Pressable } from "react-native";
import * as yup from 'yup';
import { styles } from "./styles";
import { Picker } from '@react-native-picker/picker';
import { GetCurrentUserGymResponse } from "../GymScreen/action";
import { getGymMembers, GetGymMembersResponse, TrainingCreationModel, TrainingSetCreationModel } from "./actions";
import commonStyles from "../../styles/commonStyles";
import CreateOrEditSectionComponent from "../../components/gym/CreateOrEditSectionComponent";

interface FormDataModel {
  availableGyms: GetCurrentUserGymResponse[],
  availableUsers: GetGymMembersResponse[],
  selectedGym: string | undefined,
  selectedStudent: string | undefined
}

interface TrainingGymCreationModel {
  gymId: string,
  trainingStudentId: string,
  studentName: string,
  sections: TrainingCreationModel[]
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
    .required("Insira um estudante."),
  sections: yup.array().of(
    yup.object().shape({
      muscularGroup: yup.string()
        .required("Selecione uma academia."),
      sets: yup.array().of(
          yup.object().shape({
              set: yup.string().required('A repetição é necessária.'),
              exerciseId: yup.string().required('Exercício é obrigatório.'),
          })
      ).min(1, 'Adicione ao menos um treino.')
    })
  ).min(1, 'Adicione ao menos uma seção (A, B, C, ...).')
});

const GymTrainingCreationPage = () => {

  const initialValues: TrainingGymCreationModel = {
    gymId: "",
    trainingStudentId: "",
    studentName: "",
    sections: []
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
    if (!selectedGymId) {
      resetFormData();
      return;
    }

    var membersResult = await getGymMembers(selectedGymId);

    if (membersResult.ContainsError) {
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
    data: TrainingGymCreationModel
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
                editable={!formData.selectedGym} />

              {formData.selectedGym ?
                <View>

                  <StyledInput
                    formikKey={"TrainingName"}
                    formikProps={formikProps}
                    label={"Nome do treino"}
                  />

                  {/* Trainings List */}
                  <FieldArray
                    name="Trainings"
                    render={(arrayHelpers) => (
                      <View>
                        {formikProps.values.sections.map((section, index) => (
                          <View key={index}>
                            <CreateOrEditSectionComponent
                              sectionName={section.muscularGroup}
                              section={formikProps.values.sections[index]}
                              formikProps={formikProps}
                              formikKeySection={`sections[${index}].sets[{setIndex}].exerciseId`} 
                              formikKeySet={`sections[${index}].sets[{setIndex}].set`} />

                          </View>
                        ))}
                        <Button
                          title="Adicionar"
                          onPress={() => arrayHelpers.push({ exerciseId: "", set: "" } as TrainingSetCreationModel)}
                        />
                      </View>
                    )}
                  />
                </View> :
                <View>
                  <Text>Selecione a academia e o estudante para continuar o processo.</Text>
                </View>}

              <View style={styles.buttonContainer}>
                {formikProps.isSubmitting ?
                  <ActivityIndicator /> :
                  <Pressable style={commonStyles.PrimaryButton} onPress={() => formikProps.handleSubmit()}>
                    <Text style={commonStyles.PrimaryButtonText}>Criar usuário</Text>
                  </Pressable>}
              </View>
            </React.Fragment>
          );
        }}
      </Formik>
    </SafeAreaView>
  );
}

export default GymTrainingCreationPage;