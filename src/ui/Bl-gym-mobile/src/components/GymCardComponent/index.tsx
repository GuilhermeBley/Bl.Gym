import { View, Text, Pressable, GestureResponderEvent } from "react-native";
import { styles } from "./styles";
import React from "react";


export interface GymCardInfo {
    id: string,
    name: string,
    description: string,
    createdAt: Date,
    role: string,
};

const gymRoleGroupTranslations: { [key: string]: string }[] = [
    { "Student": "Estudante" },
    { "Instructor": "Instrutor" },
    { "GymGroupOwner": "Administrador" },
]

type GymCardComponentProps = {
    item: GymCardInfo;
    onClick?: ((item: GymCardInfo) => void) | ((item: GymCardInfo) => Promise<void>) | undefined;
    isLoading?: boolean
};

const GymCardComponent: React.FC<GymCardComponentProps> = ({
    item, onClick, isLoading = false
}) => {
    return (
        <View style={styles.card}>
            <Pressable onPress={() => { if(onClick && !isLoading) return onClick(item) }} disabled={isLoading}>
                <View
                    style={styles.cardContent}>
                    <Text style={styles.cardTitle}>
                        {item.name}
                        <Text style={styles.cardText}>
                            ({translateGymRoleGroup(item.role)})
                        </Text>
                    </Text>
                    <Text style={styles.cardText}>
                        {item.description ?? "Nenhuma descrição adicionada..."}
                    </Text>
                </View>
            </Pressable>
        </View>
    );
}

function translateGymRoleGroup(roleName: string) {
    let entry = gymRoleGroupTranslations.find(item => {
        const itemKey = Object.keys(item)[0];
        return itemKey.toLowerCase() === roleName.toLowerCase();
    });

    return entry ? Object.values(entry)[0] : undefined;
}

export default GymCardComponent;