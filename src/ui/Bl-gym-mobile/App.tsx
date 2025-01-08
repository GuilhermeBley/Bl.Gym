import 'react-native-gesture-handler';

import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';
import TrainingContextProvider from './src/contexts/TrainingContext';
import { ErrorProvider } from './src/contexts/ErrorContext';
import FloatErrorComponent from './src/components/FloatErrorComponent';

/*Use the FloatErrorComponent to show the error in the top.*/
export default function App() {
  return (
    <ErrorProvider>
      <FloatErrorComponent /> 
      <UserContextProvider>
        <TrainingContextProvider>
          <Router></Router>
        </TrainingContextProvider>
      </UserContextProvider>
    </ErrorProvider>
  );
}