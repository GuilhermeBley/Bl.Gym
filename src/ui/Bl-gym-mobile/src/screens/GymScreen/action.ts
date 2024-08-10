import axios from "../../api/GymApi"
import { AxiosError, CancelToken } from "axios"
import TryGetResultFromResponse from "../../api/ResponseReader"

interface GetCurrentUserGymResponse{
    Id: string,
    Name: string,
    Description: string,
    CreatedAt: Date,
    Role: string,
};

interface GetCurrentUserGymsResponse {
    Gyms: GetCurrentUserGymResponse[]
};

export const fetchGyms = (
    userId: string,
    cancellationToken: CancelToken | undefined = undefined) => {
    return axios
        .get<GetCurrentUserGymsResponse>(
            "gym/user/{userId}".replace("{userId}", userId),
            { cancelToken: cancellationToken })
        .then(response => {
            return response.data;
        })
        .catch((error: AxiosError<GetCurrentUserGymsResponse>) => {
            
            return TryGetResultFromResponse(error.response);
        })
}