import 'react-native-gesture-handler';

import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';
import TrainingContextProvider from './src/contexts/TrainingContext';
import { ErrorProvider } from './src/contexts/ErrorContext';
import FloatErrorComponent from './src/components/FloatErrorComponent';

export default function App() {
  return (
    <ErrorProvider>
      <FloatErrorComponent /> {/*Use this to show the error in the top.*/}
      <UserContextProvider>
        <TrainingContextProvider>
          <Router></Router>
        </TrainingContextProvider>
      </UserContextProvider>
    </ErrorProvider>
  );
}