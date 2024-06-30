import React, { useState } from "react";
import { View } from "react-native";
import { UserContext } from "../contexts/UserContext";
import { useContext } from "react";

const LoginScreen = () => {

    const [userLoginInput, setUserLoginInput] = useState("");
    const [userPasswordInput, setUserPasswordInput] = useState("");

    const userContext = useContext(UserContext);

    

    return (
        <View>

        </View>
    );
};

export default LoginScreen;