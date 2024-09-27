import { StyleSheet } from "react-native"
import colors from "./colors";

const styles = StyleSheet.create({
  PageHeader: {
      fontSize: 23,
      fontWeight: 'bold',
      color: colors.primary,    
      textAlign: 'center'  
  },
  PrimaryButton: {
    backgroundColor: colors.primary,
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 3, // Adds shadow on Android
    shadowColor: '#000', // Adds shadow on iOS
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
  },
  PrimaryButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  }
});

export default styles;