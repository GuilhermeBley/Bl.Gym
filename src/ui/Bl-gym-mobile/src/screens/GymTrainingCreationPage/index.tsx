import { FieldArray, Formik, FormikHelpers, FormikProps } from "formik";
import React, { useContext, useEffect, useState } from "react";
import { SafeAreaView, TextInput, View, Text, ActivityIndicator, Button, Pressable } from "react-native";
import * as yup from 'yup';
import { styles } from "./styles";
import { Picker } from '@react-native-picker/picker';
import { GetCurrentUserGymResponse } from "../GymScreen/action";
import { GetAvailableExercisesItemResponse, getGymExercisesByPage, getGymMembers, GetGymMembersResponse, getGymsAvailables, TrainingCreationModel, TrainingSetCreationModel } from "./actions";
import commonStyles from "../../styles/commonStyles";
import CreateOrEditSectionComponent from "../../components/gym/CreateOrEditSectionComponent";
import axios from "axios";
import { UserContext } from "../../contexts/UserContext";

interface PageDataModel {
  availableGyms: GetCurrentUserGymResponse[],
  availableUsers: GetGymMembersResponse[],
  selectedGym: string | undefined,
  selectedStudent: string | undefined,
  availableTrainings: GetAvailableExercisesItemResponse[],
  isLoadingInitialData: boolean
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

interface SectionComponentProps {
  formikProps: FormikProps<TrainingGymCreationModel>;
  trainingIndex: number,
  section: TrainingCreationModel
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
          set: yup.string().required('A repetição é necessária.').min(3, "Insira uma série válida."),
          exerciseId: yup.string().required('Exercício é obrigatório.').min(3, "Insira um exercício válido."),
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

  const [pageData, setPageData] = useState(
    {
      availableGyms: [],
      availableUsers: [],
      selectedGym: undefined,
      selectedStudent: undefined,
      availableTrainings: [],
      isLoadingInitialData: true
    } as PageDataModel
  );

  const userContext = useContext(UserContext);

  useEffect(() => {
    const source = axios.CancelToken.source();

    const fetchData = async () => {
      // TODO: GET ALL USER GYM's

      var result = await getGymsAvailables(
        userContext.user.id,
        source.token);

      if (result.Success) {

        setPageData(previous => ({
          ...previous,
          Gyms: result.Data.Gyms,
          startedWithError: false,
        }));

        return;
      }
    }

    fetchData()
      .finally(() => {
        setPageData(previous => ({
          ...previous,
          isLoadingInitialData: false
        }));
      });

    return () => {
      source.cancel();
    }

  }, [])

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

  const SectionComponent: React.FC<SectionComponentProps> = ({ formikProps, trainingIndex, section: training }) => {

    return (
      <View>

        {/* Trainings List */}
        <FieldArray
          name={`sections[${trainingIndex}].sets`}
          render={(arrayHelpers) => (
            <View>
              {formikProps.values.sections[trainingIndex].sets.map((set, setIndex) => (
                <View key={setIndex}>

                  <CreateOrEditSectionComponent
                    sectionName={training.muscularGroup}
                    section={formikProps.values.sections[trainingIndex]}
                    formikProps={formikProps}
                    formikKeySection={`sections[${trainingIndex}].sets[${setIndex}].exerciseId`}
                    formikKeySet={`sections[${trainingIndex}].sets[${setIndex}].set`}
                    trainingData={new Map(pageData.availableTrainings.map(item => [item.id, item.name]))} />

                </View>
              ))}
              <Button
                title="+ Exercício"
                onPress={() => arrayHelpers.push({ exerciseId: "", set: "" } as TrainingSetCreationModel)}
              />
            </View>
          )}
        />


      </View>
    )
  }

  const resetFormData = () => {
    setPageData(previous => ({
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

    var setTrainingsAsync = async () => {
      var gymTrainingsResult = await getGymExercisesByPage(selectedGymId, 0, undefined);

      if (gymTrainingsResult.ContainsError)
      {
        return;
      }
  
      setPageData(previous => ({
        ...previous,
        availableTrainings: gymTrainingsResult.Data
      }));
    }

    var setMembersAsync = async () => {
      var membersResult = await getGymMembers(selectedGymId);

      if (membersResult.ContainsError) {
        resetFormData();
        return;
      }
  
      setPageData(previous => ({
        ...previous,
        selectedGym: selectedGymId,
        availableUsers: membersResult.Data
      }));
    }

    await Promise.all([setMembersAsync, setTrainingsAsync]);
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
                options={pageData.availableGyms.map(e =>
                  ({ label: e.Name, value: e.Id })
                )}
                autoFocus
                onValueChange={handleGymSelect} />

              <StyledSelect
                formikKey={"studentId"}
                formikProps={formikProps}
                label={"Selecione o estudante"}
                options={pageData.availableGyms.map(e =>
                  ({ label: e.Name, value: e.Id })
                )}
                editable={!pageData.selectedGym} />

              {pageData.selectedGym ?
                <View>

                  <StyledInput
                    formikKey={"TrainingName"}
                    formikProps={formikProps}
                    label={"Nome do treino"}
                  />

                  {/* Trainings Sections List */}
                  <FieldArray
                    name="sections"
                    render={(arrayHelpers) => (
                      <View>
                        {formikProps.values.sections.map((section, index) => (
                          <View key={index}>

                            <SectionComponent
                              formikProps={formikProps}
                              trainingIndex={index}
                              section={section} />

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