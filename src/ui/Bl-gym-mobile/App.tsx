import 'react-native-gesture-handler';

import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';

export default function App() {
  return (
    <UserContextProvider>
      <Router></Router>
    </UserContextProvider>
  );
}