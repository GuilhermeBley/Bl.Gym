import React from "react";
import axios from "../../api/GymApi";

enum LoginResultStatus {
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
    let response = await axios.post(
        'user/login',
        {
            Login: login,
            Password: password
        });
    
    if (response.status == 200)
    {
        return {
            Status: LoginResultStatus.Success,
            Token: response.data.token
        }
    }

    return {
        Status: LoginResultStatus.InvalidLoginOrPassword,
        Token: null
    }
}