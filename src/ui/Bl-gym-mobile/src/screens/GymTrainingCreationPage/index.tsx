import { Formik } from "formik";
import { SafeAreaView } from "react-native";

interface TrainingCreationModel {
    gymId: string,
    trainingStudentId: string,
    studentName: string,
    trainingData: undefined | TrainingCreationModel
}
  
const GymTrainingCreationPage = () => {
    const initialValues: TrainingCreationModel = {
        gymId: "",
        trainingStudentId: "",
        studentName: "",
        trainingData: undefined
    };

  return (
    <SafeAreaView style={styles.container}>
      <Formik
        initialValues={initialValues}
        onSubmit={(values, actions) => {
          console.debug("handling user creation...")

          return handleSubmit(actions, values)
              .finally(() => actions.setSubmitting(false));
        }
        }
        validationSchema={validationSchema}
      >

        {formikProps => {
          return (
            <React.Fragment>
              <StyledInput
                formikKey={"firstName"}
                formikProps={formikProps}
                label={"Primeiro nome"}
                autoFocus />

              <StyledInput
                formikKey={"lastName"}
                formikProps={formikProps}
                label={"Sobrenome"} />

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
                <Text style={{ color: 'red' }}>{formikProps.errors[responseErrorsKey as keyof UserCreateModel]}</Text>
              </View>
            </React.Fragment>
          );
        }}
      </Formik>
    </SafeAreaView>
  );
}

export default GymTrainingCreationPage;