import axios from '../../api/GymApi'

export const handleTrainings = (userId: number) => {
    return axios.get(
        "api/Training/details/{userId}".replace("{userId}", userId.toString())
    ).then((response) => {

        if (response.status !== 200)
            return {
                Success: false,
                Data: []
            }

        return {
            Success: true,
            Data: response.data
        }
    }).catch((error) => {
        console.debug(error)
        return {
            Success: false,
            Data: []
        }
    });
}