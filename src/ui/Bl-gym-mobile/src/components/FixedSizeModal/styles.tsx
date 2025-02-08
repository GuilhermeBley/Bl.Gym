import { StyleSheet } from "react-native";

const styles = StyleSheet.create({
    modalContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },
    modalContent: {
        width: 300, // Fixed width
        height: 400, // Fixed height
        backgroundColor: 'white',
        borderRadius: 10,
        padding: 20,
    },
    scrollView: {
        flex: 1,
    },
    closeButton: {
        marginTop: 10,
        padding: 10,
        backgroundColor: '#007BFF',
        borderRadius: 5,
        alignItems: 'center',
    },
    closeButtonText: {
        color: 'white',
        fontSize: 16,
    },
    title: {
      fontSize: 18,
      fontWeight: 'bold',
      marginBottom: 15,
    },
});

export default styles;