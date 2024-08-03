import { CancelToken } from "axios";
import axios  from "../../api/GymApi"

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