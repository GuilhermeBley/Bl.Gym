import React, { useState } from "react";
import { View, Text, TextInput, Button, StyleSheet } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import styles from "./styles";

const LoginScreen = () => {

    const [userLoginInput, setUserLoginInput] = useState("");
    const [userPasswordInput, setUserPasswordInput] = useState("");

    const userContext = useContext(UserContext);

    function handleLogin() {
        
    }

    return (
        <View style={styles.container}>
          <Text style={styles.title}>Gym Login</Text>
          <TextInput
            style={styles.input}
            placeholder="E-mail"
            value={userLoginInput}
            onChangeText={setUserLoginInput}
            keyboardType="email-address"
            autoCapitalize="none"
          />
          <TextInput
            style={styles.input}
            placeholder="Senha"
            value={userPasswordInput}
            onChangeText={setUserPasswordInput}
            secureTextEntry
          />
          <Button title="Login" onPress={handleLogin} />
        </View>
    );
};
  
export default LoginScreen;