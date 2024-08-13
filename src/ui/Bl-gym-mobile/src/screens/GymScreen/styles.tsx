import { StyleSheet } from "react-native"

export const styles = StyleSheet.create({
  container: {
    marginEnd: "3%",
    marginStart: "3%",
  },
  card: {
    backgroundColor: '#fff',
    borderRadius: 8,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 3,
    marginVertical: 10,
    marginHorizontal: 16,
  },
  cardContent: {
    padding: 16,
  },
  cardTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 8,
  },
  cardText: {
    fontSize: 14,
    color: '#666',
  },
  roleText: {
    fontSize: 10,
    color: '#666',
  },
  addGymButton: {

  },
  addGymButtonText: {

  },
  footerErrorMessages: {
    color: "red"
  }
});