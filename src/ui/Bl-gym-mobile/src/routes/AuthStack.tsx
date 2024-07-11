import 'react-native-gesture-handler';
import { createStackNavigator } from '@react-navigation/stack';

import LoginScreen from '../screens/LoginScreen';
import CreateUserScreen from '../screens/CreateUserScreen';

export const LOGIN_SCREEN = "Login";
export const CREATE_USER_SCREEN = "CreateUser";

const Stack = createStackNavigator();

const AuthStack = () => {
    console.debug("AuthStack");
    return (
        <Stack.Navigator initialRouteName={LOGIN_SCREEN}>
            <Stack.Screen name={LOGIN_SCREEN} component={LoginScreen} />
            <Stack.Screen name={CREATE_USER_SCREEN} component={CreateUserScreen} />
        </Stack.Navigator>
    );
}

export default AuthStack;