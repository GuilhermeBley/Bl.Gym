import 'react-native-gesture-handler';
import { createStackNavigator } from '@react-navigation/stack';

import LoginScreen from '../screens/LoginScreen';
import CreateUserScreen from '../screens/CreateUserScreen';
import { CREATE_USER_SCREEN, LOGIN_SCREEN } from './RoutesConstant';

const Stack = createStackNavigator();

const AuthStack = () => {
    console.debug("AuthStack");

    return (
        <Stack.Navigator initialRouteName={LOGIN_SCREEN}>
            <Stack.Screen name={LOGIN_SCREEN} component={LoginScreen} options={{ headerShown: false }}/>
            <Stack.Screen name={CREATE_USER_SCREEN} component={CreateUserScreen} options={{ title: "Criar novo usuário" }}/>
        </Stack.Navigator>
    );
}

export default AuthStack;