import { CancelToken } from 'axios';
import axios from '../../api/GymApi'

export const handleLoginInGym = (gymId: string) => {
    return axios.post(
        "user/login/gym/{gymId}".replace("{gymId}", gymId)
    ).then(response => {
        if (response.status == 200)
            return { Success: true };

        return { Success: false }
    }).catch(error => {
        console.debug(error)

        return { Success: false }
    })
}

export const handleTrainings = (userId: string, cancellationToken: CancelToken) => {
    return axios.get(
        "api/Training/details/{userId}".replace("{userId}", userId), {
            cancelToken: cancellationToken
        }
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