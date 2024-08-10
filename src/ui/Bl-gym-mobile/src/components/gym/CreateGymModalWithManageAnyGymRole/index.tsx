import { Modal, View, Text, Button } from "react-native";
import { styles } from "./styles";
import React from "react";

type ChildComponentProps = {
    modalVisible: boolean,
    setModalVisible: React.Dispatch<React.SetStateAction<boolean>>
};
  
const CreateGymModalWithManageAnyGymRole = ({ modalVisible, setModalVisible }: ChildComponentProps) => {
    return (
        <Modal
            animationType="slide" // You can use "slide", "fade", or "none"
            transparent={true}
            visible={modalVisible}
            onRequestClose={() => {
                setModalVisible(false);
            }}>
            <View style={styles.modalView}>
                <Text style={styles.modalText}>Hello, I'm a Modal!</Text>
                <Button title="Close Modal" onPress={() => setModalVisible(false)} />
            </View>
        </Modal>
    );
}

export default CreateGymModalWithManageAnyGymRole;