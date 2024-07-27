import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { FlatList, View, Text } from "react-native";
import { useState } from "react";

interface TrainingDataState{
    trainings: TrainingSummaryModel[]
}

interface TrainingSummaryModel {
    TrainingId: string,
    GymId: string,
    GymName: string,
    GymDescription: string,
    TrainingCreatedAt: Date,
    SectionsCount: string,
}

const TrainingCardComponent = (item : TrainingSummaryModel) => {
    return (
        <View style={styles.card}>
            <View style={styles.cardContent}>
                <Text style={styles.cardTitle}>
                    {item.GymName}
                </Text>
                <Text style={styles.cardText}>
                    {item.GymDescription}
                </Text>
            </View>
        </View>
    );
}

const TrainingListScreen = () => {

    const [trainingData, setTrainingData] = 
        useState<TrainingDataState>({
            trainings: [],
        });

    

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