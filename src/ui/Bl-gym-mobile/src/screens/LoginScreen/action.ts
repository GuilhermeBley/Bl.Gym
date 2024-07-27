import React from "react";
import axios from "../../api/GymApi";
import { Axios, AxiosError } from "axios";

export enum LoginResultStatus {
    Success,
    InvalidLoginOrPassword,
    FailedToLogin
}

interface LoginResult {
    Status: LoginResultStatus,
    Token: string | null
}

export const handleLogin = async (
    login: string,
    password: string
) => {
        return await axios.post(
            'user/login',
            {
                Login: login,
                Password: password
            })
            .then((response) => {
                return {
                    Status: LoginResultStatus.Success,
                    Token: response.data.token
                }
            })
            .catch((error: AxiosError) => {
                console.debug(error);
                return {
                    Status: LoginResultStatus.InvalidLoginOrPassword,
                    Token: null
                }
            });
}