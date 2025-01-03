import React from "react";
import { View, Text, Button } from "react-native";
import { useErrorContext } from "../../contexts/ErrorContext";
import styles from "./styles";

const FloatErrorComponent: React.FC = () => {
  const {
    errors,
    currentErrorIndex,
    closeCurrentError,
  } = useErrorContext();

  if (errors.length === 0) {
    return <View></View>;
  }

  const currentError = errors[currentErrorIndex];

  const getBackgroundColor = (type: "Warning" | "Error" | "Critical") => {
    switch (type) {
      case "Warning":
        return "#fffacd"; // Light yellow
      case "Error":
        return "#ffcccb"; // Light red
      case "Critical":
        return "#ff9999"; // Darker red
      default:
        return "#fff";
    }
  };

  return (
    <View
      style={[
        styles.container,
        { backgroundColor: getBackgroundColor(currentError.type) },
      ]}
    >
      <Text style={styles.message}>{currentError.message}</Text>
      <View style={styles.pagination}>
        <Button title="Close" onPress={closeCurrentError} />
      </View>
    </View>
  );
};

export default FloatErrorComponent;