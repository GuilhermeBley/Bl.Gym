import { useContext, useEffect, useState } from "react";
import { UserContext } from "../../contexts/UserContext";
import { ActivityIndicator, FlatList, Pressable, Text, View } from "react-native";
import { GetCurrentUserGymResponse, handleCreateGym, handleGyms } from "./action";
import axios from "axios";
import commonStyles from '../../styles/commonStyles'
import { SafeAreaView } from "react-native-safe-area-context";
import { styles } from "./styles";
import { ManageAnyGym } from "../../Constants/roleConstants";
import CreateGymModalWithManageAnyGymRole, { GymCreateModel } from "../../components/gym/CreateGymModalWithManageAnyGymRole";
import GymCardComponent from "../../components/GymCardComponent";

interface PageDataProps {
    Gyms: GetCurrentUserGymResponse[],
    errors: string[],
    startedWithError: boolean,
    isLoadingInitialData: boolean
}

const GymScreen = () => {
    const userContext = useContext(UserContext);
    const [modalVisible, setModalVisible] = useState(false)
    const [pageData, setPageData] = useState<PageDataProps>({
        Gyms: [],
        errors: [],
        startedWithError: false,
        isLoadingInitialData: true
    })

    useEffect(() => {
        const source = axios.CancelToken.source();

        const fetchData = async () => {
            var result = await handleGyms(userContext.user.id, source.token)

            if (result.Success) {

                setPageData(previous => ({
                    ...previous,
                    Gyms: result.Data.gyms,
                    startedWithError: false
                }));

                return;
            }

            setPageData(previous => ({
                ...previous,
                startedWithError: true,
                errors: result.Errors
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

    const handleGymCreation = async (model: GymCreateModel) => {
        var result = await handleCreateGym(model)

        if (result.ContainsError) {
            setPageData(previous => ({ ...previous, errors: result.Errors }))
            setModalVisible(false);
            return;
        }

        var resultGyms = await handleGyms(userContext.user.id)

        if (resultGyms.Success) {

            setPageData(previous => ({
                ...previous,
                Gyms: result.Data.Gyms,
            }));

            return;
        }
        setModalVisible(false);
    }

    const handleModalGymCreation = () => {
        setModalVisible(true)
    }

    if (pageData.isLoadingInitialData) {
        return (
            <SafeAreaView style={styles.containerCenter}>
                <View>
                    <ActivityIndicator />
                </View>
            </SafeAreaView>
        );
    }

    if (pageData.startedWithError) {
        return (
            <SafeAreaView style={styles.containerCenter}>
                <View>
                    <Text style={styles.errorText}>Falha ao inicializar página.</Text>
                </View>
            </SafeAreaView>
        );
    }

    console.debug(`showing: ${pageData.Gyms.map(e => e.id)}`)

    return (
        <SafeAreaView>

            {pageData.isLoadingInitialData ? 
                <View>
                    <ActivityIndicator/>
                </View> : 
                <View>
                    <FlatList
                        data={pageData.Gyms}
                        renderItem={(info) => <GymCardComponent item={info.item}></GymCardComponent>}
                        keyExtractor={(item) => item.id}>

                    </FlatList>
                </View>}

            {userContext.user.isInRole(ManageAnyGym)
                ? (
                    <Pressable
                        style={commonStyles.PrimaryButton}
                        onPress={() => handleModalGymCreation()}>
                        <Text style={commonStyles.PrimaryButtonText}>
                            Criar academia
                        </Text>
                    </Pressable>)
                : (<View></View>)/* Don't show nothing */}

            <CreateGymModalWithManageAnyGymRole
                modalVisible={modalVisible}
                setModalVisible={setModalVisible}
                onSubmiting={handleGymCreation} />

            {pageData.errors.map(error => (
                <Text style={styles.footerErrorMessages}>{error}</Text>
            ))}

        </SafeAreaView>
    );
}

export default GymScreen;