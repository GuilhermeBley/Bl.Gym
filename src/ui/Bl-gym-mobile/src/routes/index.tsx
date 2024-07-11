import { NavigationContainer } from "@react-navigation/native";
import AuthContainer from '../containers/AuthContainer';

import SelectGymTabs from "./SelectGymTabs";
import AuthStack from "./AuthStack";

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