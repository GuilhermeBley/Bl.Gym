import { StyleSheet } from "react-native";
import colors from "../../styles/colors";

const styles = StyleSheet.create({
  logoutButton: {
    backgroundColor: colors.danger,
    paddingVertical: 10,
    paddingHorizontal: 30,
    borderRadius: 5,
    height: "auto"
  },
  logoutText: {
    color: '#FFF',
    fontSize: 16,
    textAlign: "center",
    fontWeight: '600',
  }
});
  
export default styles;