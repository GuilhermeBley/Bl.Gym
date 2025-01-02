import { StyleSheet } from "react-native";



const styles = StyleSheet.create({
  container: {
    padding: 20,
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    backgroundColor: "#fff",
  },
  type: {
    fontSize: 18,
    fontWeight: "bold",
    marginBottom: 10,
  },
  message: {
    fontSize: 16,
    marginBottom: 20,
  },
  pagination: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  pageIndicator: {
    fontSize: 14,
    fontWeight: "bold",
  },
  warning: {
    color: "orange",
  },
  error: {
    color: "red",
  },
  critical: {
    color: "darkred",
  },
});

  
export default styles;