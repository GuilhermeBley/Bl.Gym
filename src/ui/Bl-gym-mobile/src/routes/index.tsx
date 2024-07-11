import { NavigationContainer } from "@react-navigation/native";
import AuthContainer from '../containers/AuthContainer';

import SelectGymTabs from "./SelectGymTabs";
import AuthStack from "./AuthStack";

export const LOGIN_SCREEN = "Login";
export const CREATE_USER_SCREEN = "CreateUser";
export const HOME_SCREEN = "Home";

const Router = () => {

    return (
        <NavigationContainer>
            <AuthContainer
                authorizedContent={(<SelectGymTabs/>)}
                unauthorizedContent={(<AuthStack/>)}
            />
        </NavigationContainer>
    )
}

export default Router;