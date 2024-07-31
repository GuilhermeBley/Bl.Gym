import { useContext } from "react";
import { Text, View } from "react-native";
import { TrainingContext } from "../../contexts/TrainingContext";
import { UserContext } from "../../contexts/UserContext";

const TrainingScreen = () => {
    const trainingContext = useContext(TrainingContext)
    const userContext = useContext(UserContext)

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