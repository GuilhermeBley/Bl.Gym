import { NavigationContainer } from "@react-navigation/native";
import AuthContainer from '../containers/AuthContainer';

import AuthorizedGymTabs from "./AuthorizedGymTabs";
import AuthStack from "./AuthStack";
import AuthGymStack from "./AuthGymStack";

const Router = () => {

    return (
        <NavigationContainer>
            <AuthContainer
                authorizedContent={(<AuthGymStack/>)}
                authorizedGymContent={(<AuthorizedGymTabs/>)}
                unauthorizedContent={(<AuthStack/>)}
            />
        </NavigationContainer>
    )
}

export default Router;