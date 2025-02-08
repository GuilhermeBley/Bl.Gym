import React from 'react';
import { Modal, View, ScrollView, StyleSheet, TouchableOpacity, Text } from 'react-native';
import styles from './styles';


interface FixedSizeModalProps {
  visible: boolean;
  onClose?: () => void;
  title?: string;
  children: React.ReactNode;
}

const FixedSizeModal: React.FC<FixedSizeModalProps> = ({ visible, onClose, title, children }) => {
  return (
    <Modal
      visible={visible}
      transparent={true}
      animationType="slide"
      onRequestClose={onClose}
    >
      <View style={styles.modalContainer}>
        {title && <Text style={styles.title}>{title}</Text>}
        <View style={styles.modalContent}>
          <ScrollView style={styles.scrollView}>
            {children}
          </ScrollView>
          <TouchableOpacity style={styles.closeButton} onPress={onClose}>
            <Text style={styles.closeButtonText}>Fechar</Text>
          </TouchableOpacity>
        </View>
      </View>
    </Modal>
  );
};

export default FixedSizeModal;