import { NavigationContainer } from "@react-navigation/native";
import AuthContainer from '../containers/AuthContainer';

import SelectGymTabs from "./SelectGymTabs";
import AuthStack from "./AuthStack";
import AuthGymStack from "./AuthGymStack";

const Router = () => {

    return (
        <NavigationContainer>
            <AuthContainer
                authorizedContent={(<AuthGymStack/>)}
                authorizedGymContent={(<SelectGymTabs/>)}
                unauthorizedContent={(<AuthStack/>)}
            />
        </NavigationContainer>
    )
}

export default Router;