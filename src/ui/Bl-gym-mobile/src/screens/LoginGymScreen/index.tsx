import { useContext, useEffect, useState } from "react";
import { ActivityIndicator, FlatList, Pressable, View, Text } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import GymCardComponent, { GymCardInfo } from "../../components/GymCardComponent";
import { getGyms, handleLogin, LoginResultStatus } from "./action";
import { UserContext } from "../../contexts/UserContext";
import axios from 'axios';
import styles from "./styles";
import MiddleFloatLoadingScreen from "../../components/MiddleFloatLoadingScreen";

const LoginGymScreen = () => {

    const [pageData, setPageData] = useState<{
        isLoadingInitialData: boolean
        isLoadingLogin: boolean
        topErrors: string[]
        Gyms: GymCardInfo[]
    }>({
        isLoadingInitialData: true,
        isLoadingLogin: false,
        topErrors: [],
        Gyms: []
    });

    const handleGymLogin = async (
        gymId: string
    ) => {

        if (pageData.isLoadingLogin)
            return;

        try{
            setPageData(prev => ({
                ...prev,
                isLoadingLogin: true
            }));

            let result = await handleLogin(gymId);
    
            if (result.Status == LoginResultStatus.Success)
            {
                await login(result.Token, result.RefreshToken);

                return;
            }

            setPageData(prev => ({
                ...prev,
                topErrors: ['Falha ao realizar Login em academia.']
            }));
        }
        finally{
            setPageData(prev => ({
                ...prev,
                isLoadingLogin: false
            }));
        }
    }

    const { user, login, logout } = useContext(UserContext);

    useEffect(() => {

        var src = axios.CancelToken.source();

        const handleInitialPageLoading = async () => {

            try
            {
                var gymsResult = await getGyms(user.id);
    
                if (!gymsResult.Success)
                {
                    setPageData(prev => ({
                        ...prev,
                        topErrors: gymsResult.Errors
                    }));
                }

                setPageData(prev => ({
                    ...prev,
                    Gyms: gymsResult?.Data?.gyms ?? []
                }));
            }
            finally
            {
                setPageData(prev => ({
                    ...prev,
                    isLoadingInitialData: false
                }));
            }
        };

        handleInitialPageLoading();

        return () => { src.cancel(); }
    }, [])

    return (
        <SafeAreaView>

            {pageData.isLoadingLogin 
                ? <MiddleFloatLoadingScreen/>
                : <View></View>}

            {pageData.isLoadingInitialData ? 
                <View>
                    <ActivityIndicator/>
                </View> : 
                <View>
                    <FlatList
                        data={pageData.Gyms ?? []}
                        renderItem={(info) =>
                            <GymCardComponent
                                item={info.item}
                                onClick={(item) => handleGymLogin(item.id)}
                                isLoading={pageData.isLoadingLogin}>
                            </GymCardComponent>}
                        keyExtractor={(item) => item.id}>

                    </FlatList>
                </View>}

                
            <Pressable style={styles.logoutButton} onPress={logout}>
                <Text style={styles.logoutText}>Sair</Text>
            </Pressable>
        </SafeAreaView>
    );
}

export default LoginGymScreen;