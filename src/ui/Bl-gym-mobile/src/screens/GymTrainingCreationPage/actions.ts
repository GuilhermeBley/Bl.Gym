import axios from "axios"
import TryGetResultFromResponse from "../../api/ResponseReader";


export const handleTrainingCreation = (
    trainingStudentId: string,
    gymId: string,
    trainingData: TrainingCreationModel 
) => {
    return axios.post(
        "Training",
        {
            gymId: gymId,
            studentId: trainingStudentId,
            Sets: trainingData
        }
    ).then((response) => {

        return TryGetResultFromResponse(response);
    })
    .catch((error) => {
        console.debug(error)
        return TryGetResultFromResponse(error.response);
    });
}

export type TrainingCreationModel = {
    muscularGroup: string,
    sets: TrainingSetCreationModel[]
}

export type TrainingSetCreationModel = {
    set: string,
    exerciseId: string
}