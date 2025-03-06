import { View, Text, Button } from 'react-native';
import { FormikProps, FieldArray } from 'formik';
import FilteredInputSelect from '../../FilteredInputSelect';
import StyledInputFormik from '../../StyledInputFormik';

interface TrainingDataToEditSectionModel {
    sections: TrainingCreationModel[]
}

interface TrainingCreationModel{
    muscularGroup: string,
    sets: TrainingSetCreationModel[]
}

interface TrainingSetCreationModel{
    set: string,
    exerciseId: string
}

interface CreateOrEditSectionComponentProps {
    trainingIndex: number,
    section: TrainingCreationModel | undefined,
    formikProps: FormikProps<any>,
    trainingData?: Map<string, string>,
    onLoadingMoreTrainingData?: () => Promise<any>
}

const CreateOrEditSectionComponent: React.FC<CreateOrEditSectionComponentProps> = ({
    trainingIndex,
    section,    
    formikProps,
    trainingData = new Map<string, string>(),
    onLoadingMoreTrainingData = () => { }
}) => {

    let sections = formikProps.values.sections as TrainingCreationModel[] ?? [];

    section = section ?? ({ muscularGroup: '', sets: [] });

    return (
        <FieldArray
          name={`sections[${trainingIndex}].sets`}
          render={(arrayHelpers) => (
            <View>
              {(sections[trainingIndex]?.sets ?? []).map((set, setIndex) => (

                <View key={setIndex}>
                    <View>
                        <StyledInputFormik
                            formikKey={`sections[${trainingIndex}].sets[${setIndex}].set`}
                            formikProps={formikProps} 
                            label={"Coloque as repetições"}/>

                        <FilteredInputSelect
                            data={trainingData}
                            formikKey={`sections[${trainingIndex}].sets[${setIndex}].exerciseId`}
                            formikProps={formikProps}
                            label="Adicione um treino"
                            placeHolder="Digite um treino..."
                        />
                    </View>
                </View>
              ))}
              <Button
                title="+ Exercício"
                onPress={() => arrayHelpers.push({ exerciseId: "", set: "" } as TrainingSetCreationModel)}
              />
            </View>
          )}
        />
    )
}

export default CreateOrEditSectionComponent;