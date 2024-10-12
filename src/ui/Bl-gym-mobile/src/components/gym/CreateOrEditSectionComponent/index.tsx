import * as yup from 'yup';
import { TrainingCreationModel } from "../../../screens/GymTrainingCreationPage/actions";

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

const CreateOrEditSectionComponent = (
    section: TrainingCreationModel | undefined
) => {
    section = section ?? ({muscularGroup: '', sets: [] });
}

export default CreateOrEditSectionComponent;