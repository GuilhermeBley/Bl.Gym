import { useContext, useEffect, useState } from "react";
import { ActivityIndicator, FlatList, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import GymCardComponent, { GymCardInfo } from "../../components/GymCardComponent";
import { getGyms, handleLogin, LoginResultStatus } from "./action";
import { UserContext } from "../../contexts/UserContext";
import axios from 'axios';

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

        try{
            setPageData(prev => ({
                ...prev,
                isLoadingLogin: true
            }));

            let result = await handleLogin(gymId);
    
            if (result.Status == LoginResultStatus.Success)
            {
                await userCtx.login(result.Token, result.RefreshToken);
    
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

    const userCtx = useContext(UserContext);

    useEffect(() => {

        var src = axios.CancelToken.source();

        const handleInitialPageLoading = async () => {

            try
            {
                var gymsResult = await getGyms(userCtx.user.id);
    
                if (!gymsResult.Success)
                {
                    setPageData(prev => ({
                        ...prev,
                        topErrors: gymsResult.Errors
                    }));
                }

                setPageData(prev => ({
                    ...prev,
                    Gyms: gymsResult.Data.gyms
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
            {pageData.isLoadingInitialData ? 
                <View>
                    <ActivityIndicator/>
                </View> : 
                <View>
                    <FlatList
                        data={pageData.Gyms}
                        renderItem={(info) => <GymCardComponent item={info.item} onClick={}></GymCardComponent>}
                        keyExtractor={(item) => item.id}>

                    </FlatList>
                </View>}
        </SafeAreaView>
    );
}

export default LoginGymScreen;