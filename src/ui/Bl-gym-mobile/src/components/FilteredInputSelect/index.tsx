import React, { useState } from 'react';
import { View, TextInput, FlatList, Text, TouchableOpacity } from 'react-native';
import styles from './styles';
import { FormikProps } from 'formik';

interface FilteredInputSelectProps {
  data: string[];
  formikKey: string,
  formikProps: FormikProps<any>;
  label: string;
  [key: string]: any;
}

const FilteredInputSelect : React.FC<FilteredInputSelectProps> = (
  { data, formikKey, formikProps, label, rest }
) => {

  const [filteredData, setFilteredData] = useState(data);
  const [showDropdown, setShowDropdown] = useState(false);

  // Function to filter data based on input
  const handleInputChange = (text: string) => {
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

    formikProps.handleChange(formikKey)
  };

  // Function to handle item selection
  const handleSelectItem = (item: string) => {
    setShowDropdown(false); // Hide the dropdown after selection
  };
  
  const inputStyles = styles.input;

  const error = formikProps.touched[formikKey] && formikProps.errors[formikKey];
  const errorMessage = typeof error === 'string' ? error : '';

  if (errorMessage) {
    inputStyles.borderColor = "red"
  }

  return (
    <View style={styles.container}>
      <Text>{label}</Text>
      <TextInput
        style={inputStyles}
        value={formikProps.values[formikKey]}
        onChangeText={handleInputChange}
        onBlur={formikProps.handleBlur(formikKey)}
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
      {errorMessage ? <Text style={{ color: 'red' }}>{errorMessage}</Text> : null}
    </View>
  );
};

export default FilteredInputSelect;