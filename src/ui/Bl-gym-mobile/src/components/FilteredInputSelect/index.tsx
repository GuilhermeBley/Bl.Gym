import React, { useState } from 'react';
import { View, TextInput, FlatList, Text, TouchableOpacity } from 'react-native';
import styles from './styles';

interface FilteredInputSelectProps {
  data: string[];
  onChange: (text: string) => void;
  [key: string]: any;
}

const FilteredInputSelect : React.FC<FilteredInputSelectProps> = (
  { data, onChange, rest }
) => {

  const [inputText, setInputText] = useState('');
  const [filteredData, setFilteredData] = useState(data);
  const [showDropdown, setShowDropdown] = useState(false);

  // Function to filter data based on input
  const handleInputChange = (text: string) => {
    setInputText(text);
    if (text) {
      const filtered = data.filter((item) => 
        item.toLowerCase().includes(text.toLowerCase())
      );
      setFilteredData(filtered);
      setShowDropdown(true);
    } else {
      setFilteredData(data);
      setShowDropdown(false);
    }

    onChange(text);
  };

  // Function to handle item selection
  const handleSelectItem = (item: string) => {
    setInputText(item); // Set the selected item in the input
    setShowDropdown(false); // Hide the dropdown after selection
  };

  return (
    <View style={styles.container}>
      <TextInput
        style={styles.input}
        value={inputText}
        onChangeText={handleInputChange}
        {...rest}
      />
      {showDropdown && (
        <FlatList
          data={filteredData}
          keyExtractor={(item) => item}
          style={styles.dropdown}
          renderItem={({ item }) => (
            <TouchableOpacity onPress={() => handleSelectItem(item)}>
              <Text style={styles.item}>{item}</Text>
            </TouchableOpacity>
          )}
        />
      )}
    </View>
  );
};

export default FilteredInputSelect;