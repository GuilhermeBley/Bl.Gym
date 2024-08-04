import axios from "../../api/GymApi"

interface RequestToChangePasswordProps {
    email: string
}

export const handleRequestToChangePassword = (
    user: RequestToChangePasswordProps
) => {

    return axios
        .post("user/change-password/request", {
            email: ""
        })
        .then(response => {
            return {
                Success: true
            }
        })
}