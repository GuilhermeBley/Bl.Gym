import axios from "../../api/GymApi"
import { CancelToken } from "axios"
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
        .get<GetCurrentUserGymsResponse>("gym/user/{userId}".replace("{userId}", userId))
        .then(response => {
            return response.data;
        })
        .catch(error => {
            return TryGetResultFromResponse(error);
        })
}