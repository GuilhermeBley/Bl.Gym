import 'react-native-gesture-handler';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';

import TrainingListScreen from '../screens/TrainingListScreen';
import { GYM_SCREEN, HOME_SCREEN, TRAINING_SCREEN } from './RoutesConstant';
import TrainingScreen from '../screens/TrainingScreen';
import GymScreen from '../screens/GymScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function HomeStackNavigator() {
  return (
    <Stack.Navigator initialRouteName={HOME_SCREEN}>
      <Stack.Screen name={HOME_SCREEN} component={TrainingListScreen} options={{ title: "Meus treinos" }} />
      <Stack.Screen name={TRAINING_SCREEN} component={TrainingScreen} options={{ title: "Meus treinos" }} />
    </Stack.Navigator>
  );
}

function SelectGymTabs() {
  return (
    <Tab.Navigator initialRouteName={HOME_SCREEN}>
      <Tab.Screen name={HOME_SCREEN} component={HomeStackNavigator} options={{ headerShown: false }}/>
      <Tab.Screen name={GYM_SCREEN} component={GymScreen} options={{ headerShown: false }}/>
    </Tab.Navigator>
  );
}

export default SelectGymTabs;