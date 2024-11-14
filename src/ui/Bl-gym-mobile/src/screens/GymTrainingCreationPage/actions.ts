import { CancelToken } from "axios"
import axios from "../../api/GymApi";
import TryGetResultFromResponse from "../../api/ResponseReader";
import { handleGyms } from "../GymScreen/action";

export const handleTrainingCreation = (
    trainingStudentId: string,
    gymId: string,
    trainingData: TrainingCreationModel[] 
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
    
    console.debug(`Requesting in gym/{gymId}/members`);

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

export const getGymExercisesByPage = (
    gymId: string,
    page: number = 0,
    cancellationToken: CancelToken | undefined = undefined) => { 
        
    const maxPageSize = 1000;

    console.debug(`Training/exercises/gym/{gymId}?skip={skip}&take={take}`);

    return axios.get<GetAvailableExercisesResponse>(
        "Training/exercises/gym/{gymId}?skip={skip}&take={take}"
            .replace("{gymId}", gymId)
            .replace("{skip}", (maxPageSize * page).toString())
            .replace("{take}", maxPageSize.toString()),
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

export interface GetAvailableExercisesResponse {
    availableExercises: GetAvailableExercisesItemResponse[];
}

export interface GetAvailableExercisesItemResponse {
    id: string;
    name: string;
    description: string;
}
