import { useState } from "react";
import { ActivityIndicator, FlatList, View } from "react-native";
import { SafeAreaView } from "react-native-safe-area-context";
import GymCardComponent, { GymCardInfo } from "../../components/GymCardComponent";

const LoginGymScreen = () => {

    const [pageData, setPageData] = useState<{
        isLoadingInitialData: boolean
        Gyms: GymCardInfo[]
    }>({
        isLoadingInitialData: true,
        Gyms: []
    });

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