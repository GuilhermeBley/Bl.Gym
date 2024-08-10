import { useContext, useEffect, useState } from "react";
import { UserContext } from "../../contexts/UserContext";
import { FlatList, Pressable, Text, View } from "react-native";
import { GetCurrentUserGymResponse, handleGyms } from "./action";
import axios from "axios";
import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { ManageAnyGym } from "../../Constants/roleConstants";
import CreateGymModalWithManageAnyGymRole from "../../components/gym/CreateGymModalWithManageAnyGymRole";

interface PageDataProps {
    Gyms: GetCurrentUserGymResponse[],
    errors: string[]
}

const GymCardComponent = (item: GetCurrentUserGymResponse) => {
    return (
        <View style={styles.card}>
            <View
                style={styles.cardContent}>
                <Text style={styles.cardTitle}>
                    {item.Name}
                </Text>
                <Text style={styles.cardText}>
                    {item.Description}
                </Text>
                <Text style={styles.roleText}>
                    {item.Role}
                </Text>
            </View>
        </View>
    );
}

const GymScreen = () => {
    const userContext = useContext(UserContext);
    const [modalVisible, setModalVisible] = useState(false)
    const [pageData, setPageData] = useState<PageDataProps>({
        Gyms: [],
        errors: []
    })

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchData = async () => {
            var result = await handleGyms(userContext.user.id, source.token)

            if (result.Success) {

                setPageData(previous => ({
                    ...previous,
                    Gyms: result.Data.Gyms
                }));

                return;
            }

            setPageData(previous => ({
                ...previous,
                errors: ["Falha ao coletar dados dos treinos."]
            }));
        }

        fetchData();

        return () => {
            source.cancel();
        }
    }, [])

    const handleModalGymCreation = () => {
        setModalVisible(true)
    }

    return (
        <SafeAreaView>
            <FlatList
                data={pageData.Gyms}
                renderItem={(info) => GymCardComponent(info.item)}
                keyExtractor={(item) => item.Id}>

            </FlatList>

            {userContext.user.isInRole(ManageAnyGym)
                ? (
                    <Pressable
                        style={styles.addGymButton}
                        onPress={() => handleModalGymCreation()}>
                        <Text style={styles.addGymButtonText}>
                            Criar academia
                        </Text>
                    </Pressable>)
                : (<View></View>)/* Don't show nothing */}
            
            <CreateGymModalWithManageAnyGymRole
                modalVisible={modalVisible}
                setModalVisible={setModalVisible} />
        </SafeAreaView>
    );
}

export default GymScreen;