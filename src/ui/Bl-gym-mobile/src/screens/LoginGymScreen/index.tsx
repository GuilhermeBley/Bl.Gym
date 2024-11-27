import { useContext, useEffect, useState } from "react";
import { ActivityIndicator, FlatList, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import GymCardComponent, { GymCardInfo } from "../../components/GymCardComponent";
import { getGyms } from "./action";
import { UserContext } from "../../contexts/UserContext";
import axios from 'axios';

const LoginGymScreen = () => {

    const [pageData, setPageData] = useState<{
        isLoadingInitialData: boolean
        initialErrors: string[]
        Gyms: GymCardInfo[]
    }>({
        isLoadingInitialData: true,
        initialErrors: [],
        Gyms: []
    });

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
                        initialErrors: gymsResult.Errors
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
                        renderItem={(info) => <GymCardComponent item={info.item}></GymCardComponent>}
                        keyExtractor={(item) => item.id}>

                    </FlatList>
                </View>}
        </SafeAreaView>
    );
}

export default LoginGymScreen;