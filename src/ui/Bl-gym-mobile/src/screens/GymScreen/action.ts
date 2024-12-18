import axios from "../../api/GymApi"
import { AxiosError, CancelToken } from "axios"
import TryGetResultFromResponse from "../../api/ResponseReader"
import { GymCreateModel } from "../../components/gym/CreateGymModalWithManageAnyGymRole"

export interface GetCurrentUserGymResponse{
    id: string,
    name: string,
    description: string,
    createdAt: Date,
    role: string,
};

export interface GetCurrentUserGymsResponse {
    gyms: GetCurrentUserGymResponse[]
};

export const handleGyms = (
    userId: string,
    cancellationToken: CancelToken | undefined = undefined) => {
    return axios
        .get<GetCurrentUserGymsResponse>(
            "gym/user/{userId}".replace("{userId}", userId),
            { cancelToken: cancellationToken })
        .then(response => {
            return TryGetResultFromResponse(response);
        })
        .catch((error: AxiosError<GetCurrentUserGymsResponse>) => {
            console.debug('api -> gym/user')
            console.debug(error);
            return TryGetResultFromResponse(error.response);
        })
}

export const handleCreateGym = (
    model: GymCreateModel,
    cancellationToken: CancelToken | undefined = undefined) => {
    return axios
        .post(
            "gym/createAsAdmin",
            {
                Name: model.gymName,
                Description: model.description,
            },
            { cancelToken: cancellationToken })
        .then(response => {
            return TryGetResultFromResponse(response);
        })
        .catch((error: AxiosError) => {
            console.debug(error);
            return TryGetResultFromResponse(error.response);
        })
}