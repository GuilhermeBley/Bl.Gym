import React, { useContext } from "react";
import { UserContext } from "../contexts/UserContext";

interface AuthContainerProps {
    authorizedContent: React.ReactNode;
    authorizedGymContent: React.ReactNode;
    unauthorizedContent: React.ReactNode;
}

const AuthContainer: React.FC<AuthContainerProps> = ({
    authorizedContent,
    unauthorizedContent,
    authorizedGymContent
}) => {
    
    const { user } = useContext(UserContext);

    if (user.isAuthorized() && !user.isExpirated() && user.isAuthorizedInGym()) {
        console.log(`User authorized in gym ${user.gymId}.`);
        return authorizedGymContent;
    }

    if (user.isAuthorized() && !user.isExpirated()) {
        console.log("User authorized.");
        return authorizedContent;
    }

    console.log("User unauthorized.")
    return unauthorizedContent;
}

export default AuthContainer;