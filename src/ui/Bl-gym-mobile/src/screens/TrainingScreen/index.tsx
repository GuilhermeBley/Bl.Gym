import { useContext, useEffect, useState } from "react";
import { Text, View } from "react-native";
import { TrainingContext } from "../../contexts/TrainingContext";
import { UserContext } from "../../contexts/UserContext";
import axios from "axios";
import { getTrainingInfoById, GetTrainingInfoByIdResponse } from "./action";

interface TrainingInfoModel{
    errors: string[],
    data: GetTrainingInfoByIdResponse | undefined
}

const TrainingScreen = () => {
    const [trainingInfo, setTrainingInfo] = useState<TrainingInfoModel>({
        errors: [],
        data: undefined
    });

    const trainingContext = useContext(TrainingContext)
    const userContext = useContext(UserContext)

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchTraning = async () => {

            if (trainingContext.training === undefined)
                return;

            let response = await getTrainingInfoById(
                trainingContext.training.trainingId,
                userContext.user.id,
                source.token);
            
            if (response.ContainsError)
            {
                setTrainingInfo(previous => ({
                    ...previous,
                    errors: response.Errors
                }))
                return;
            }
            
            setTrainingInfo(previous => ({
                ...previous,
                data: response.Data
            }))
            
        }

        fetchTraning();

        return () => {
            source.cancel();
        }

    }, [])

    return trainingContext.training === undefined ?
        (
            <View>
                <Text>Usuário não autorizado para visualizar este treino.</Text>
            </View>
        ) :
        (
            <View>

            </View>
        );
}

export default TrainingScreen;