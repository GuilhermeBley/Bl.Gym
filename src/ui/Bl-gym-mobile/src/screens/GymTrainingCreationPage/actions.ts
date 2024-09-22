import axios, { CancelToken } from "axios"
import TryGetResultFromResponse from "../../api/ResponseReader";
import { handleGyms } from "../GymScreen/action";


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

export const getGymsAvailables = (
    userId: string,
    cancellationToken: CancelToken | undefined = undefined) => {
    return handleGyms(userId, cancellationToken);
}

export const getGymMembers = (
    gymId: string,
    cancellationToken: CancelToken | undefined = undefined) => {
    
    return axios.get<GetGymMembersResponse>(
        "gym/{gymId}/members".replace("{gymId}", gymId),
        { cancelToken: cancellationToken }
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

export interface GetGymMembersResponse {
    students: GetGymMembersItemResponse[]
}

export interface GetGymMembersItemResponse {
    userId: string;
    email: string;
    name: string;
    lastName: string;
    roleName: string;
}