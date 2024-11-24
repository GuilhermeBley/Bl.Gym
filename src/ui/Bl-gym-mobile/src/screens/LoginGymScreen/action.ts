
import axios from "../../api/GymApi";
import { Axios, AxiosError, CancelToken } from "axios";
import { GetCurrentUserGymsResponse, handleGyms } from "../GymScreen/action";
import TryGetResultFromResponse from "../../api/ResponseReader";

export enum LoginResultStatus {
    Success,
    FailedToLogin
}

export const getGyms = (
    userId: string,
    cancellationToken: CancelToken | undefined = undefined) => {
    return handleGyms(userId, cancellationToken);
}

export const handleLogin = async (
    gymId: string
)=>{
        return await axios.post(
            'user/login/gym',
            {
                GymId: gymId
            })
            .then((response) => {
                return {
                    Status: LoginResultStatus.Success,
                    Token: response.data.token,
                    RefreshToken: response.data.refreshToken
                }
            })
            .catch((error: AxiosError) => {
                console.debug(error);
                return {
                    Status: LoginResultStatus.FailedToLogin,
                    Token: null,
                    RefreshToken: null
                }
            });
}