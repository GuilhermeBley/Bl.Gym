import { CancelToken } from 'axios';
import axios from '../../api/GymApi'
import TryGetResultFromResponse from '../../api/ResponseReader';

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
        "Training/details/{userId}".replace("{userId}", userId), {
            cancelToken: cancellationToken
        }
    ).then((response) => {
        
        return TryGetResultFromResponse(response)
    }).catch((error) => {
        console.debug(error)
        return TryGetResultFromResponse(error.response);
    });
}

export const handleTrainingCreation = (
    currentUserId: string,
    trainingUserId: string,
    trainingData: TrainingCreationModel 
) => {
    
}

export type TrainingCreationModel = {

}