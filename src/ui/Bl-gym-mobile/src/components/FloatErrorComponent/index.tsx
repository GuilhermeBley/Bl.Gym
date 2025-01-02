import React, { useState } from "react";
import { View, Text, Button } from "react-native";
import styles from "./styles";

interface Error {
  type: "Warning" | "Error" | "Critical";
  message: string;
}

interface FloatErrorViewerProps {
  errors: Error[];
}

const FloatErrorViewer: React.FC<FloatErrorViewerProps> = ({ errors }) => {
  const [currentIndex, setCurrentIndex] = useState<number>(0);

  const nextError = () => {
    setCurrentIndex((prev) => (prev + 1) % errors.length);
  };

  const prevError = () => {
    setCurrentIndex((prev) => (prev - 1 + errors.length) % errors.length);
  };

  const currentError = errors[currentIndex];

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
        <Button title="Previous" onPress={prevError} />
        <Text style={styles.pageIndicator}>
          {currentIndex + 1} / {errors.length}
        </Text>
        <Button title="Next" onPress={nextError} />
      </View>
    </View>
  );
};