import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { FlatList, View, Text, Pressable, ActivityIndicator } from "react-native";
import { useContext, useEffect, useState } from "react";
import { handleTrainings } from "./action";
import { UserContext } from "../../contexts/UserContext";
import axios from "axios";
import { TrainingContext } from "../../contexts/TrainingContext";
import { TRAINING_SCREEN } from "../../routes/RoutesConstant";
import commonStyles from "../../styles/commonStyles";

interface TrainingDataState{
    trainings: TrainingSummaryModel[],
    errors: string[],
    startedWithError: boolean,
    isLoadingInitialData: boolean
}

interface TrainingSummaryModel {
    TrainingId: string,
    GymId: string,
    GymName: string,
    GymDescription: string,
    TrainingCreatedAt: Date,
    SectionsCount: string,
}

const TrainingListScreen = ({ navigation }: any) => {

    const userContext = useContext(UserContext);
    const trainingContext = useContext(TrainingContext);

    const [pageData, setPageData] = 
        useState<TrainingDataState>({
            trainings: [],
            errors: [],
            startedWithError: false,
            isLoadingInitialData: true,
        });

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchData = async () => {
            var result = await handleTrainings(userContext.user.id, source.token)
            
            if (result.Success){
                
                setPageData(previous => ({
                    ...previous,
                    trainings: result.Data,
                    startedWithError: false,
                }));
                
                return;
            }

            setPageData(previous => ({
                ...previous,
                errors: [...previous.errors, "Falha ao coletar dados dos treinos."],
                startedWithError: true
            }));
        } 

        fetchData()
            .finally(() => {
                setPageData(previous => ({
                    ...previous,
                    isLoadingInitialData: false
                }));  
            });

        return () => {
            source.cancel();
        }
    }, [])

    const navigateToTrainingPage = (trainingId: string) => {
        trainingContext.setTrainingContext({
            trainingDescription: 'Meu treino',
            trainingId: trainingId,
            trainingName: 'Meu treino'
        })

        navigation.navigate(TRAINING_SCREEN)
    }

    const TrainingCardComponent = (item : TrainingSummaryModel) => {
        return (
            <View style={styles.card}>
                <Pressable
                    style={styles.cardContent}
                    onPress={() => navigateToTrainingPage(item.TrainingId)}>
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

    if (pageData.isLoadingInitialData){
        return (
            <SafeAreaView style={styles.containerCenter}>
                <View>
                    <ActivityIndicator />
                </View>
            </SafeAreaView>
        );
    }

    if (pageData.startedWithError){
        return (
            <SafeAreaView style={styles.containerCenter}>
                <View>
                    <Text  style={styles.errorText}>Falha ao inicializar página.</Text>
                </View>
            </SafeAreaView>
        );
    }

    return(
        <SafeAreaView style={styles.container}>
            
            <FlatList
                data={pageData.trainings}
                renderItem={(info) => TrainingCardComponent(info.item)}
                keyExtractor={(item) => item.TrainingId}
            />
        </SafeAreaView>
    )
};

export default TrainingListScreen;