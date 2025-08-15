import { StyleSheet } from "react-native";

const styles = StyleSheet.create({
    container: {
      flex: 1,
      justifyContent: 'center',
      padding: 20,
    },
    title: {
      fontSize: 24,
      marginBottom: 20,
      textAlign: 'center',
    },
    buttonContainer: {
      marginTop: 20
    },
    inputContainer: {
      marginBottom: 12,
    },
    input: {
      marginBottom: 5,
      backgroundColor: 'transparent', // For better theming
    },
    button: {
      marginVertical: 10,
      paddingVertical: 5,
    },
    errorText: {
      marginTop: 10,
      textAlign: 'center',
    },
});

export default styles;