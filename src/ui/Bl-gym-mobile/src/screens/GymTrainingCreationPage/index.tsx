import { FieldArray, Formik, FormikErrors, FormikHelpers, FormikProps } from "formik";
import React, { useContext, useEffect, useState } from "react";
import { SafeAreaView, TextInput, View, Text, ActivityIndicator, Button, Pressable } from "react-native";
import * as yup from 'yup';
import { styles } from "./styles";
import { GetCurrentUserGymResponse } from "../GymScreen/action";
import { 
  GetAvailableExercisesItemResponse, 
  getGymExercisesByPage, 
  getGymMembers, 
  GetGymMembersItemResponse, 
  getGymsAvailables, 
  handleTrainingCreation, 
  TrainingCreationModel, 
  TrainingSetCreationModel 
} from "./actions";
import commonStyles from "../../styles/commonStyles";
import CreateOrEditSectionComponent from "../../components/gym/CreateOrEditSectionComponent";
import axios from "axios";
import { UserContext } from "../../contexts/UserContext";
import StyledInputFormik from "../../components/StyledInputFormik";
import StyledSelectInputFormik from "../../components/StyledSelectInputFormik";

interface PageDataModel {
  availableGyms: GetCurrentUserGymResponse[],
  availableUsers: GetGymMembersItemResponse[],
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

interface SectionComponentProps {
  formikProps: FormikProps<TrainingGymCreationModel>;
  trainingIndex: number,
  section: TrainingCreationModel
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
  const responseErrorsKey = "api-errors"

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
          availableGyms: result.Data.gyms,
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

    console.debug('Requesting setMembersAsync and setTrainingsAsync')
    await Promise.all([setMembersAsync(), setTrainingsAsync()]);
  }

  const handleSubmit = async (
    formikHelper: FormikHelpers<any>,
    data: TrainingGymCreationModel
  ) => {
    var result = await handleTrainingCreation(
      data.trainingStudentId,
      data.gymId,
      data.sections,
    )

    result.Errors.forEach(error => {
      formikHelper.setFieldError(responseErrorsKey, error)
    });
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
              <StyledSelectInputFormik
                formikKey={"gymId"}
                formikProps={formikProps}
                label={"Academia"}
                options={pageData.availableGyms.map(e =>
                  ({ label: e.name, value: e.id })
                )}
                autoFocus
                onValueChange={handleGymSelect} />

              <StyledSelectInputFormik
                formikKey={"studentId"}
                formikProps={formikProps}
                label={"Selecione o estudante"}
                options={pageData.availableUsers.map(e =>
                  ({ label: (e.name + ' ' + e.lastName), value: e.userId })
                )}
                editable={!pageData.selectedGym} />

              {pageData.selectedStudent && pageData.selectedGym ?
                <View>

                  <StyledInputFormik
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
                    <Text style={commonStyles.PrimaryButtonText}>Criar treino</Text>
                  </Pressable>}
              </View>

              <View>
                <Text style={{ color: 'red' }}>{formikProps.errors[responseErrorsKey as keyof TrainingGymCreationModel]?.toString()}</Text>
              </View>
            </React.Fragment>
          );
        }}
      </Formik>
    </SafeAreaView>
  );
}

export default GymTrainingCreationPage;