import { NavigationContainer } from "@react-navigation/native";

import NotLoggedStack from "./NotLoggedStack";

const Router = () => {
    return (
        <NavigationContainer>
            <NotLoggedStack />
        </NavigationContainer>
    )
}

export default Router;