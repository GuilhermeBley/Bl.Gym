import 'react-native-gesture-handler';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import TrainingListScreen from '../screens/TrainingListScreen';
import { HOME_SCREEN } from './RoutesConstant';

const Tab = createBottomTabNavigator();

function SelectGymTabs() {
  return (
    <Tab.Navigator>
      <Tab.Screen name={HOME_SCREEN} component={TrainingListScreen} />
    </Tab.Navigator>
  );
}

export default SelectGymTabs;