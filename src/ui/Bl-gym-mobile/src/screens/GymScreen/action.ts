import axios from "../../api/GymApi"
import { CancelToken } from "axios"

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
        .get<GetCurrentUserGymsResponse>("")
        .then(response => {
            
        })
        .then(error => {

        })
}