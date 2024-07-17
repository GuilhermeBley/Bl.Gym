import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { FlatList, View, Text } from "react-native";

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

    const trainings: TrainingSummaryModel[] = [];

    <SafeAreaView style={styles.container}>
        <FlatList
            data={trainings}
            renderItem={(info) => TrainingCardComponent(info.item)}
            keyExtractor={(item) => item.TrainingId}
        />
    </SafeAreaView>
};

export default TrainingListScreen;