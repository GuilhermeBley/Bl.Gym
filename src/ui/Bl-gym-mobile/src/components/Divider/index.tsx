import { View, StyleSheet } from "react-native";

const styles = StyleSheet.create({
    divider: {
        height: 1,
        width: '100%',
        backgroundColor: 'black',
        marginVertical: 5,
    },
});

const Divider = () => {
    return (
        <View style={styles.divider}></View>
    );
}

export default Divider;