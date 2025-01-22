import React, { useState, useRef, ReactNode } from 'react';
import { Text, View, TouchableOpacity, Animated } from 'react-native';
import { styles } from './styles';

interface AccordionItemProps {
  title: string;
  body: ReactNode;
}

interface AccordionProps {
  sections: AccordionItemProps[] | AccordionItemProps;
}

const AccordionComponent: React.FC<AccordionProps> = ({ sections }) => {
  const [activeSection, setActiveSection] = useState<number | null>(null);

  if (!Array.isArray(sections)) sections = [sections];

  const animatedHeights = sections.map(() => new Animated.Value(0));

  const toggleSection = (index: number) => {
    if (activeSection === index) {
      // Collapse current section
      Animated.timing(animatedHeights[index], {
        toValue: 0,
        duration: 300,
        useNativeDriver: false,
      }).start();
      setActiveSection(null);
    } else {
      // Collapse any open section
      if (activeSection !== null) {
        Animated.timing(animatedHeights[activeSection], {
          toValue: 0,
          duration: 300,
          useNativeDriver: false,
        }).start();
      }
      // Expand the new section
      Animated.timing(animatedHeights[index], {
        toValue: 150, // Adjust to your content height
        duration: 300,
        useNativeDriver: false,
      }).start();
      setActiveSection(index);
    }
  };

  return (
    <View style={styles.container}>
      {sections.map((section, index) => (
        <View key={index} style={styles.section}>
          <TouchableOpacity onPress={() => toggleSection(index)} style={styles.header}>
            <Text style={styles.headerText}>{section.title}</Text>
          </TouchableOpacity>
          <Animated.View style={[styles.body, { height: animatedHeights[index] }]}>
            <View>{section.body}</View>
          </Animated.View>
        </View>
      ))}
    </View>
  );
};

export default AccordionComponent;