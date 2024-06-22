import { createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from '../src/screens/HomeScreen';

const Stack = createNativeStackNavigator();

function NotLoggedStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Home" component={HomeScreen} />
    </Stack.Navigator>
  );
}

export default NotLoggedStack;