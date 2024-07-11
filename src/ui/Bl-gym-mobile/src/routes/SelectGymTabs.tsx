import HomeScreen from '../screens/HomeScreen';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import { HOME_SCREEN } from '.';

const Tab = createBottomTabNavigator();

function SelectGymTabs() {
  return (
    <Tab.Navigator>
      <Tab.Screen name={HOME_SCREEN} component={HomeScreen} />
    </Tab.Navigator>
  );
}

export default SelectGymTabs;