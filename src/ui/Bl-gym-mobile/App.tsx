import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';
import LoginScreen from './src/screens/LoginScreen';

export default function App() {
  return (
    <UserContextProvider>
      <Router></Router>
    </UserContextProvider>
  );
}