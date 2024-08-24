import { AxiosError, CancelToken } from "axios";
import axios from "../../api/GymApi"
import TryGetResultFromResponse, { createFailedResponse } from "../../api/ResponseReader";

export interface GetTrainingInfoByIdResponse{
    Section: GetTrainingInfoByIdResponseSection[],
    Status: string,
    CreatedAt: Date
}

export interface GetTrainingInfoByIdResponseSection{
    SectionId: string,
    MuscularGroup: string,
    TargetDaysCount: number,
    CurrentDaysCount: number,
    ConcurrencyStamp: string,
    Sets: GetTrainingInfoByIdResponseExerciseSet[],
    CreatedAt: Date,
}

export interface GetTrainingInfoByIdResponseExerciseSet{
    Set: string,
    Title: string,
    Description: string,
    CreatedAt: Date,
}

export const getTrainingInfoById = (
    sheetId: string,
    userId: string,
    cancellationToken: CancelToken
) => {
    return axios
        .get<GetTrainingInfoByIdResponse>(
            "Training/details/{userId}/sheet/{sheetId}"
                .replace("{userId}", userId)
                .replace("{sheetId}", sheetId),
            { cancelToken: cancellationToken })
        .then(response => {
            return TryGetResultFromResponse(response)
        })
        .catch((error: AxiosError<GetTrainingInfoByIdResponse>) => {
            return TryGetResultFromResponse(error.response)
        })
}

export const patchCurrentTrainingDays = async (
    sectionId: string,
    newCurrentDaysCount: number,
    cancellationToken: CancelToken
) => {
    if (newCurrentDaysCount < 0)
    {
        return createFailedResponse("Dias completos de treino não pode ser igual a zero.");
    }

    return await axios
        .patch("Training/{sectionId}/update-current-training-days",
            {
                SectionId: sectionId,
                NewCurrentDaysCount: newCurrentDaysCount,
            },
            { cancelToken: cancellationToken })
        .then(response => {
            return TryGetResultFromResponse(response);
        })
        .catch((error: AxiosError) => {
            return TryGetResultFromResponse<any>(error.response);
        })
}