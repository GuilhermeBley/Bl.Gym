import * as yup from 'yup';
import { TrainingCreationModel } from "../../../screens/GymTrainingCreationPage/actions";
import { View, Text } from 'react-native';
import { FormikProps } from 'formik';
import FilteredInputSelect from '../../FilteredInputSelect';
import { useEffect, useState } from 'react';

interface ComponentData {
    trainingData: string[]   
}

interface CreateOrEditSectionComponentProps {
    sectionName: string,
    section: TrainingCreationModel | undefined,
    index: number,
    formikProps: FormikProps<any>
}

const validationSchema = yup.object().shape({
    muscularGroup: yup.string()
        .required("Selecione uma academia."),
    sets: yup.array().of(
        yup.object().shape({
            set: yup.string().required('A repetição é necessária.'),
            exerciseId: yup.string().required('Exercício é obrigatório.'),
        })
    ).min(1, 'Adicione ao menos um treino.')
});

const CreateOrEditSectionComponent : React.FC<CreateOrEditSectionComponentProps> = ({sectionName, section, index, formikProps}) => {
    section = section ?? ({ muscularGroup: '', sets: [] });
    const [compoenentData, SetCompoenentData] = useState(
        {
            trainingData: [],
        } as ComponentData
    )
    
    useEffect(
        () => {
            

            return () => {
                // cancels the component data load
            };
        },
        [])

    return (
        <View>
            <Text>{sectionName}</Text>
            
            <FilteredInputSelect
                data={[]}
                formikKey={`sections[${index}].street`}
                formikProps={formikProps}
                label="Adicione um treino"
                placeHolder="Digite um treino..."
            />
        </View>
    )
}

export default CreateOrEditSectionComponent;