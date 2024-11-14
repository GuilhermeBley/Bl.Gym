import { TrainingCreationModel } from "../../../screens/GymTrainingCreationPage/actions";
import { View, Text } from 'react-native';
import { FormikProps } from 'formik';
import FilteredInputSelect from '../../FilteredInputSelect';
import StyledInputFormik from '../../StyledInputFormik';

interface CreateOrEditSectionComponentProps {
    sectionName: string,
    section: TrainingCreationModel | undefined,
    formikProps: FormikProps<any>,
    trainingData?: Map<string, string>,
    formikKeySet: string,
    formikKeySection: string,
    onLoadingMoreTrainingData?: () => Promise<any>
}

const CreateOrEditSectionComponent: React.FC<CreateOrEditSectionComponentProps> = ({
    sectionName,
    section,    
    formikProps,
    trainingData = new Map<string, string>(),
    formikKeySet,
    formikKeySection,
    onLoadingMoreTrainingData = () => { }
}) => {
    section = section ?? ({ muscularGroup: '', sets: [] });

    return (
        <View>
            <Text>{sectionName}</Text>
            
            <StyledInputFormik
                formikKey={formikKeySet}
                formikProps={formikProps} 
                label={"Coloque as repetições"}/>

            <FilteredInputSelect
                data={trainingData}
                formikKey={formikKeySection}
                formikProps={formikProps}
                label="Adicione um treino"
                placeHolder="Digite um treino..."
            />
        </View>
    )
}

export default CreateOrEditSectionComponent;