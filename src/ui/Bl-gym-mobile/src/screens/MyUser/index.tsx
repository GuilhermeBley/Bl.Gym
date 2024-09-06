import React, { useEffect, useState } from "react";
import { View, Text, TextInput, Button, ActivityIndicator } from "react-native";
import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import commonStyles from "../../styles/commonStyles";
import styles from "./styles";
import { SafeAreaView } from "react-native-safe-area-context";

interface PageData {
  isRunningFirstLoading: boolean
}

const MyUser = ({ navigation }: any) => {

  const {  user } = useContext(UserContext);
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

  console.debug("LoginScreen");

  if (pageData.isRunningFirstLoading)
  {
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
     
    </SafeAreaView>
  );
};

export default MyUser;