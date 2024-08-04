import 'react-native-gesture-handler';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';

import TrainingListScreen from '../screens/TrainingListScreen';
import { HOME_SCREEN, TRAINING_SCREEN } from './RoutesConstant';
import TrainingScreen from '../screens/TrainingScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function HomeStackNavigator() {
  return (
    <Stack.Navigator initialRouteName={HOME_SCREEN}>
      <Stack.Screen name={HOME_SCREEN} component={TrainingListScreen} />
      <Stack.Screen name={TRAINING_SCREEN} component={TrainingScreen} />
    </Stack.Navigator>
  );
}

function SelectGymTabs() {
  return (
    <Tab.Navigator>
      <Tab.Screen name={HOME_SCREEN} component={HomeStackNavigator} />
    </Tab.Navigator>
  );
}

export default SelectGymTabs;