import AsyncStorage from "@react-native-async-storage/async-storage";

const KEY = "Authorization"

// Storing data
export const storeAuthorization = async (token: string) => {
    try {
        await AsyncStorage.setItem(KEY, token);
    } catch (e) {
        console.error('Error storing data: ', e);
        return undefined;
    }
};

// Retrieving data
export const getAuthorization = async () => {
    try {
        return await AsyncStorage.getItem(KEY);
    } catch (e) {
        console.error('Error retrieving data: ', e);
        return undefined;
    }
};