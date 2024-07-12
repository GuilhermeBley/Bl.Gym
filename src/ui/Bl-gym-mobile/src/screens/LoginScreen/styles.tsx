import { StyleSheet } from "react-native";

const styles = StyleSheet.create({
  container: {
    marginEnd: "3%",
    marginStart: "3%",
    height: "100%",
    justifyContent: 'center',
    marginTop: "auto",
  },
  title: {
    fontSize: 24,
    marginBottom: 16,
    textAlign: 'center',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    paddingHorizontal: 8,
  },
  inputContainer: {
    marginBottom: 12,
  },
  buttonContainer: {
    marginTop: 20
  },
  linkButton: {
    backgroundColor: 'transparent',
    width: "100%",
    alignItems: "center",
    marginTop: 30
  },
  linkText: {
    color: 'blue',
    textDecorationLine: 'underline',
  },
});

export default styles;