import React, { useState } from 'react';
import {
  View,
  Text,
  Button,
  Modal,
  ActivityIndicator,
} from 'react-native';
import styles from './styles';

const App = () => {
  const [loading, setLoading] = useState(false);

  const toggleLoading = () => {
    setLoading(!loading);
  };

  return (
    <View style={styles.container}>
      <Button title="Carregando..." onPress={toggleLoading} />
      {/* Loading Modal */}
      <Modal
        transparent={true}
        animationType="fade"
        visible={loading}
        onRequestClose={() => setLoading(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <ActivityIndicator size="large" color="#0000ff" />
            <Text style={styles.loadingText}>Carregando...</Text>
          </View>
        </View>
      </Modal>
    </View>
  );
};

export default App;