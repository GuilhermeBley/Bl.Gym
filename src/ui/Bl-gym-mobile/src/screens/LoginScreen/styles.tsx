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
  },
  inputContainer: {
    marginBottom: 12,
  },
  buttonContainer: {
    marginTop: 20
  },
  separatorContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginVertical: 10,
  },
  line: {
    flex: 1,
    height: 1,
    backgroundColor: 'black',
  },
  separatorText: {
    marginHorizontal: 10,
  },
});

export default styles;