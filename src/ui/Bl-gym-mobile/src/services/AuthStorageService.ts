import AsyncStorage from "@react-native-async-storage/async-storage";

const KEY_TOKEN = "Authorization"
const KEY_REFRESH_TOKEN = "RefreshToken"

// Storing data
export const storeAuthorization = async (token: string) => {
    try {
        await AsyncStorage.setItem(KEY_TOKEN, token);
    } catch (e) {
        console.error('Error storing data: ', e);
        return undefined;
    }
};

// Retrieving data
export const getAuthorization = async () => {
    try {
        return await AsyncStorage.getItem(KEY_TOKEN);
    } catch (e) {
        console.error('Error retrieving data: ', e);
        return undefined;
    }
};

// Storing data
export const storeRefreshToken = async (token: string) => {
    try {
        await AsyncStorage.setItem(KEY_REFRESH_TOKEN, token);
    } catch (e) {
        console.error('Error storing data: ', e);
        return undefined;
    }
};

// Retrieving data
export const getRefreshToken = async () => {
    try {
        return await AsyncStorage.getItem(KEY_REFRESH_TOKEN);
    } catch (e) {
        console.error('Error retrieving data: ', e);
        return undefined;
    }
};