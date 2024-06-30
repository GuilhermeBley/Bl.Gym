import React, { useContext } from "react";
import { UserContext } from "../contexts/UserContext";
import { View } from "react-native";

interface AuthContainerProps {
    authorizedContent: React.ReactNode;
    unauthorizedContent: React.ReactNode;
}

const AuthContainer: React.FC<AuthContainerProps> = ({
    authorizedContent, unauthorizedContent }) => {
    
    const userContext = useContext(UserContext);

    if (userContext.authorized) {
        console.log("User authorized.")
        return (
            <View>
                {authorizedContent}
            </View>
        )
    }

    console.log("User unauthorized.")
    return (
        <View>
            {unauthorizedContent}
        </View>
    );
}

export default AuthContainer;