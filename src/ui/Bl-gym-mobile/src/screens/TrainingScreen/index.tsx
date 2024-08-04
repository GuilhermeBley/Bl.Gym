import { useContext, useEffect, useState } from "react";
import { Text, View } from "react-native";
import { TrainingContext } from "../../contexts/TrainingContext";
import { UserContext } from "../../contexts/UserContext";
import axios from "axios";
import { getTrainingInfoById } from "./action";

const TrainingScreen = () => {
    const [trainingInfo, setTrainingInfo] = useState({
        data: []
    });

    const trainingContext = useContext(TrainingContext)
    const userContext = useContext(UserContext)

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchTraning = async () => {

            if (trainingContext.training === undefined)
                return;

            let trainings = await getTrainingInfoById(
                trainingContext.training.trainingId,
                source.token);
            
            setTrainingInfo(previous => ({
                ...previous,
                data: trainings
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