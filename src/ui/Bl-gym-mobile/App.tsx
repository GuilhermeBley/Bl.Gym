import 'react-native-gesture-handler';

import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';
import TrainingContextProvider from './src/contexts/TrainingContext';

export default function App() {
  return (
    <UserContextProvider>
      <TrainingContextProvider>
        <Router></Router>
      </TrainingContextProvider>
    </UserContextProvider>
  );
}