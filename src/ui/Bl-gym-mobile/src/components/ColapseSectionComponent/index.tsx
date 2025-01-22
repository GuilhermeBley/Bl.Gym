import React, { useState, useRef, ReactNode } from 'react';
import { Text, View, TouchableOpacity, Animated } from 'react-native';
import { styles } from './styles';

interface CollapsibleSectionProps {
  body: ReactNode;
}

const ColapseSectionComponent: React.FC<CollapsibleSectionProps> = ({ body }) => {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const animationHeight = useRef(new Animated.Value(0)).current;

  const toggleCollapse = () => {
    if (isCollapsed) {
      // Expand the section
      Animated.timing(animationHeight, {
        toValue: 150, // Change to the desired height of the expanded section
        duration: 300,
        useNativeDriver: false,
      }).start();
    } else {
      // Collapse the section
      Animated.timing(animationHeight, {
        toValue: 0,
        duration: 300,
        useNativeDriver: false,
      }).start();
    }
    setIsCollapsed(!isCollapsed);
  };

  return (
    <View style={styles.container}>
      <TouchableOpacity onPress={toggleCollapse} style={styles.button}>
        <Text style={styles.buttonText}>
          {isCollapsed ? 'Show More' : 'Show Less'}
        </Text>
      </TouchableOpacity>

      <Animated.View style={[styles.collapsibleSection, { height: animationHeight }]}>
        <View>{body}</View>
      </Animated.View>
    </View>
  );
};

export default ColapseSectionComponent;