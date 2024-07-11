import 'react-native-gesture-handler';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import HomeScreen from '../screens/HomeScreen';

export const HOME_SCREEN = "Home";

const Tab = createBottomTabNavigator();

function SelectGymTabs() {
  return (
    <Tab.Navigator>
      <Tab.Screen name={HOME_SCREEN} component={HomeScreen} />
    </Tab.Navigator>
  );
}

export default SelectGymTabs;