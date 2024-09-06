import React from "react";
import axios from "../../api/GymApi";
import { Axios, AxiosError } from "axios";
import AsyncStorage from "@react-native-async-storage/async-storage";

export enum LoginResultStatus {
    Success,
    InvalidLoginOrPassword,
    FailedToLogin
}

interface LoginResult {
    Status: LoginResultStatus,
    Token: string | null,
    RefreshToken: string | null,
}

export const handleRefreshToken = async (
    refreshToken: string,
    userId: string
) => {
    return await axios.post(
        'user/refresh',
        {
            UserId: userId,
            RefreshToken: refreshToken
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
                Status: LoginResultStatus.InvalidLoginOrPassword,
                Token: null,
                RefreshToken: null
            }
        });
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
                    Token: response.data.token,
                    RefreshToken: response.data.refreshToken
                }
            })
            .catch((error: AxiosError) => {
                console.debug(error);
                return {
                    Status: LoginResultStatus.InvalidLoginOrPassword,
                    Token: null,
                    RefreshToken: null
                }
            });
}