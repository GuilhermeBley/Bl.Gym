import React, { useEffect, useState } from "react";
import { View, Text, TextInput, Button, ActivityIndicator, Pressable } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import commonStyles from "../../styles/commonStyles";
import styles from "./styles";
import { SafeAreaView } from "react-native-safe-area-context";

interface PageData {
  isRunningFirstLoading: boolean
}

const MyUser = ({ navigation }: any) => {

  const { user, logout } = useContext(UserContext);
  const [pageData, setPageData] = useState({
    isRunningFirstLoading: false
  } as PageData)

  useEffect(() => {

    let fetchInitialData = async () => {
      setPageData(previous => ({
        ...previous,
        isRunningFirstLoading: true
      }));

    }

    fetchInitialData()
      .finally(() => {
        setPageData(previous => ({
          ...previous,
          isRunningFirstLoading: false
        }));
      })
  }, [])

  const onLogout = async () => {
    await logout();
  }

  console.debug("LoginScreen");

  if (pageData.isRunningFirstLoading) {
    return (
      <SafeAreaView style={styles.container}>
        <View>
          <ActivityIndicator></ActivityIndicator>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>

      <View style={styles.userInfo}>
        <Text style={styles.label}>Nome:</Text>
        <Text style={styles.value}>{user.name}</Text>

        <Text style={styles.label}>E-mail:</Text>
        <Text style={styles.value}>{user.email}</Text>
      </View>

      <View style={styles.separatorContainer}>
        <View style={styles.line} />
      </View>

      <Pressable style={styles.logoutButton} onPress={onLogout}>
        <Text style={styles.logoutText}>Sair</Text>
      </Pressable>
    </SafeAreaView>
  );
};

export default MyUser;