import 'react-native-gesture-handler';
import { createStackNavigator } from '@react-navigation/stack';
import { LOGIN_SCREEN, CREATE_USER_SCREEN } from '.';

import LoginScreen from '../screens/LoginScreen';
import CreateUserScreen from '../screens/CreateUserScreen';

const Stack = createStackNavigator();

const AuthStack = () => (
    <Stack.Navigator>
        <Stack.Screen name={LOGIN_SCREEN} component={LoginScreen} />
        <Stack.Screen name={CREATE_USER_SCREEN} component={CreateUserScreen} />
    </Stack.Navigator>
);

export default AuthStack;