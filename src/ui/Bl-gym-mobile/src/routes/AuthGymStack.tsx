import 'react-native-gesture-handler';
import { createStackNavigator } from '@react-navigation/stack';

import LoginGymScreen from '../screens/LoginGymScreen';

import { LOGIN_GYM_SCREEN } from './RoutesConstant';

const Stack = createStackNavigator();

const AuthGymStack = () => {
    return (
        <Stack.Navigator initialRouteName={LOGIN_GYM_SCREEN}>
            <Stack.Screen name={LOGIN_GYM_SCREEN} component={LoginGymScreen} options={{ headerShown: false }}/>
        </Stack.Navigator>
    );
}

export default AuthGymStack;