import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { FlatList, View, Text, Pressable } from "react-native";
import { useContext, useEffect, useState } from "react";
import { handleTrainings } from "./action";
import { UserContext } from "../../contexts/UserContext";
import axios from "axios";

interface TrainingDataState{
    trainings: TrainingSummaryModel[],
    errors: string[]
}

interface TrainingSummaryModel {
    TrainingId: string,
    GymId: string,
    GymName: string,
    GymDescription: string,
    TrainingCreatedAt: Date,
    SectionsCount: string,
}

const TrainingListScreen = () => {

    const userContext = useContext(UserContext);

    const [trainingData, setTrainingData] = 
        useState<TrainingDataState>({
            trainings: [],
            errors: []
        });

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchData = async () => {
            var result = await handleTrainings(userContext.user.id, source.token)
            
            if (result.Success) {
                trainingData.trainings.push(result.Data)
                return;
            }

            trainingData.errors.push("Falha ao coletar dados dos treinos.");
        } 

        fetchData();

        return () => {
            source.cancel();
        }
    }, [])

    const TrainingCardComponent = (item : TrainingSummaryModel) => {
        return (
            <View style={styles.card}>
                <Pressable style={styles.cardContent} onPress={navigateToTrainingPage}>
                    <Text style={styles.cardTitle}>
                        {item.GymName}
                    </Text>
                    <Text style={styles.cardText}>
                        {item.GymDescription}
                    </Text>
                </Pressable>
            </View>
        );
    }

    const navigateToTrainingPage = () => {
        
    }

    return(
        <SafeAreaView style={styles.container}>
            
            <View>
                <Text>Lista de treinos: </Text>
            </View>
            <FlatList
                data={trainingData.trainings}
                renderItem={(info) => TrainingCardComponent(info.item)}
                keyExtractor={(item) => item.TrainingId}
            />
        </SafeAreaView>
    )
};

export default TrainingListScreen;