import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { FlatList, View } from "react-native";

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
        <View>

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