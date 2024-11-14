import 'react-native-gesture-handler';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';

import TrainingListScreen from '../screens/TrainingListScreen';
import { GYM_SCREEN, HOME_SCREEN, MY_USER_SCREEN, TRAINING_HOME_SCREEN, TRAINING_SCREEN, TRAINING_CREATION_SCREEN } from './RoutesConstant';
import TrainingScreen from '../screens/TrainingScreen';
import GymScreen from '../screens/GymScreen';
import commonStyles from '../styles/commonStyles';
import MyUser from '../screens/MyUser';
import GymTrainingCreationPage from '../screens/GymTrainingCreationPage';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function HomeStackNavigator() {
  return (
    <Stack.Navigator initialRouteName={TRAINING_HOME_SCREEN}>
      <Stack.Screen name={TRAINING_HOME_SCREEN} component={TrainingListScreen} options={{ title: "Meus treinos", headerTitleStyle: commonStyles.PageHeader }} />
      <Stack.Screen name={TRAINING_SCREEN} component={TrainingScreen} options={{ title: "Meus treinos", headerTitleStyle: commonStyles.PageHeader }} />
      <Stack.Screen name={TRAINING_CREATION_SCREEN} component={GymTrainingCreationPage} options={{title: "Criar treino", headerTitleStyle: commonStyles.PageHeader }} />
    </Stack.Navigator>
  );
}

function SelectGymTabs() {
  return (
    <Tab.Navigator initialRouteName={HOME_SCREEN}>
      <Tab.Screen name={HOME_SCREEN} component={HomeStackNavigator} options={{ headerShown: false }} />
      <Tab.Screen name={GYM_SCREEN} component={GymScreen} options={{ tabBarLabel: "Academia", title: "Minhas academias", headerTitleStyle: commonStyles.PageHeader }} />
      <Tab.Screen name={MY_USER_SCREEN} component={MyUser} options={{ title: "Meu usuário", headerTitleStyle: commonStyles.PageHeader }} />
    </Tab.Navigator>
  );
}

export default SelectGymTabs;