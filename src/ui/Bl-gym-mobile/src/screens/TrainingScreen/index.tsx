import { useContext, useEffect, useState } from "react";
import { FlatList, Text, View } from "react-native";
import { TrainingContext } from "../../contexts/TrainingContext";
import { UserContext } from "../../contexts/UserContext";
import axios from "axios";
import { getTrainingInfoById, GetTrainingInfoByIdResponse, GetTrainingInfoByIdResponseSection } from "./action";

interface TrainingInfoModel{
    errors: string[],
    data: GetTrainingInfoByIdResponse | undefined,
    selectedSection: GetTrainingInfoByIdResponseSection | undefined
}

// This component should have a 'start training' button to redirect to section training.
const SectionComponent = (section: GetTrainingInfoByIdResponseSection) => {
    return (
        <View>

        </View>
    );
}

const TrainingScreen = () => {
    const [trainingInfo, setTrainingInfo] = useState<TrainingInfoModel>({
        errors: [],
        data: undefined,
        selectedSection: undefined
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
                {trainingInfo.data === undefined ?
                    <View>Carregando informações do treino...</View> :
                    // training info:
                    <View>
                        <Text>{trainingInfo.data.Status}</Text>
                        <Text>Treino feito por {11 /* Somar dias de treino*/} dias</Text>

                        <FlatList
                            data={trainingInfo.data.Section}
                            renderItem={(info) => SectionComponent(info.item)}
                            keyExtractor={(item) => item.SectionId}/>
                    </View>
                }
            </View>
        );
}

export default TrainingScreen;