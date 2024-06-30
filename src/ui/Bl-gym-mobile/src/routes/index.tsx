import { NavigationContainer } from "@react-navigation/native";
import { useContext } from "react";

import SelectGymTabs from "./SelectGymTabs";
import { UserContext } from "../contexts/UserContext";

const Router = () => {
    
    const userContext = useContext(UserContext);
    
    return (
        <NavigationContainer>
            <SelectGymTabs />
        </NavigationContainer>
    )
}

export default Router;