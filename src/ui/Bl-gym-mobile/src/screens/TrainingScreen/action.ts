import { AxiosError, CancelToken } from "axios";
import axios from "../../api/GymApi"
import TryGetResultFromResponse from "../../api/ResponseReader";

export const getTrainingInfoById = (
    trainingId: string,
    cancellationToken: CancelToken
) => {
    return axios
        .get("", { cancelToken: cancellationToken })
        .then(response => {
            return []
        })
}

export const patchCurrentTrainingDays = (
    sectionId: string,
    newCurrentDaysCount: number,
    cancellationToken: CancelToken
) => {
    if (newCurrentDaysCount < 0)
    {
        // TODO: Returns an error
    }

    return axios
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