import { StyleSheet } from "react-native";

export const styles = StyleSheet.create({
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
    input: {
      height: 40,
      borderColor: 'gray',
      borderWidth: 1,
      paddingHorizontal: 8,
    },
    buttonContainer: {
      marginTop: 20
    },
    inputContainer: {
      marginBottom: 12,
    },
    rowSectionItem: {
      flexDirection: 'row',
      alignItems: 'center',
      justifyContent: 'space-between',
      padding: 10,
      marginVertical: 5,
      borderWidth: 1,
      borderColor: '#ccc',
      borderRadius: 5,
      backgroundColor: '#f9f9f9',
    },
    textSectionItem: {
      flex: 1,
      textAlign: 'center',
      fontSize: 16,
    },
    editButtonSectionItem: {
      paddingVertical: 5,
      paddingHorizontal: 10,
      backgroundColor: '#007BFF',
      borderRadius: 5,
    },
    editText: {
      color: '#fff',
      fontWeight: 'bold',
    },
});