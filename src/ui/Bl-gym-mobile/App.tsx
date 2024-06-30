import Router from './src/routes/index';
import UserContextProvider from './src/contexts/UserContext';
import AuthContainer from './src/containers/AuthContainer';
import LoginScreen from './src/screens/LoginScreen';

export default function App() {
  return (
    <UserContextProvider>
      <AuthContainer
        authorizedContent={(<Router />)}
        unauthorizedContent={(<LoginScreen />)}
      />
    </UserContextProvider>
  );
}